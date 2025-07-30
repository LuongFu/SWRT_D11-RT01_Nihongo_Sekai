"use strict";

let privateConnection = null;
let activePrivateUserId = null;

// === Khởi tạo SignalR cho Chat Riêng ===
function initPrivateChat() {
    privateConnection = new signalR.HubConnectionBuilder()
        .withUrl("/privateChatHub")
        .withAutomaticReconnect()
        .build();

    privateConnection.start()
        .then(() => console.log("[PrivateChat] Connected to hub"))
        .catch(err => console.error("[PrivateChat] Connection error:", err));

    // Lắng nghe tin nhắn realtime từ server
    privateConnection.on("ReceivePrivateMessage", (senderId, message, sentAt, isOwn) => {
        if (!activePrivateUserId || (senderId !== activePrivateUserId && senderId !== window.currentUserId)) {
            console.log("[PrivateChat] New message from:", senderId);
        }

        appendPrivateMessage({
            userId: senderId,
            message: message,
            sentAt: sentAt,
            avatarUrl: senderId === window.currentUserId
                ? (window.currentUserAvatar || "/uploads/profile/default-img.jpg")
                : getUserAvatar(senderId),
            senderName: senderId === window.currentUserId
                ? (window.currentUserName || "Tôi")
                : getUserName(senderId)
        });
    });
}

// === Gửi tin nhắn ===
async function sendPrivateMessage() {
    const input = document.getElementById("privateChatInput");
    const message = input.value.trim();

    if (!message) {
        console.warn("[PrivateChat] Message is empty");
        return;
    }
    if (!activePrivateUserId) {
        alert("Hãy chọn người để chat riêng.");
        return;
    }

    const payload = {
        classroomId: window.classroomId,
        targetUserId: activePrivateUserId,
        message: message
    };

    try {
        const response = await fetch("/ClassroomInstances/SendPrivateMessage", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        const result = await response.json();
        if (result.success) {
            console.log("[PrivateChat] Message sent:", result.message);
            input.value = "";
        } else {
            console.error("[PrivateChat] Error:", result.error);
            alert(result.error || "Không thể gửi tin nhắn.");
        }
    } catch (err) {
        console.error("[PrivateChat] Send error:", err);
    }
}

// === Thêm tin nhắn vào UI ===
function appendPrivateMessage(msg) {
    const container = document.getElementById("privateChatMessages");
    const isOwn = msg.userId === window.currentUserId;

    const bubble = document.createElement("div");
    bubble.className = `chat-bubble ${isOwn ? "own" : ""}`;
    bubble.innerHTML = `
        <img src="${msg.avatarUrl}" class="chat-avatar" alt="${msg.senderName}" />
        <div class="chat-content">
            <p class="mb-1"><strong>${msg.senderName}:</strong> ${escapeHtml(msg.message)}</p>
            <small class="text-muted">${msg.sentAt}</small>
        </div>
    `;
    container.appendChild(bubble);
    container.scrollTop = container.scrollHeight;
}

// === Load tin nhắn từ API ===
async function loadPrivateMessages(targetUserId) {
    try {
        const response = await fetch(`/ClassroomInstances/GetPrivateChatMessages?classroomId=${window.classroomId}&targetUserId=${targetUserId}`);
        const messages = await response.json();

        const container = document.getElementById("privateChatMessages");
        container.innerHTML = "";

        messages.forEach(msg => {
            appendPrivateMessage({
                userId: msg.userId,
                message: msg.message,
                sentAt: msg.sentAt,
                avatarUrl: msg.avatarUrl,
                senderName: msg.senderName
            });
        });
    } catch (err) {
        console.error("[PrivateChat] Load messages error:", err);
    }
}

// === Khi click "Chat" bên Thành viên lớp ===
function handleStartPrivateChat(button) {
    const userId = button.getAttribute("data-user-id");
    const userName = button.getAttribute("data-user-name");

    if (!userId || userId === window.currentUserId) return;

    const chatTab = document.querySelector('a[href="#private-chat"]');
    if (chatTab) {
        const tabInstance = new bootstrap.Tab(chatTab);
        tabInstance.show();
    }

    const userList = document.getElementById("privateChatUserList");
    let existing = userList.querySelector(`li[data-user-id="${userId}"]`);
    if (!existing) {
        const li = document.createElement("li");
        li.className = "list-group-item start-private-chat";
        li.setAttribute("data-user-id", userId);
        li.setAttribute("data-user-name", userName);
        li.innerHTML = `<img src="/uploads/profile/default-img.jpg" class="rounded-circle me-2" width="28" height="28" /> ${userName}`;
        userList.appendChild(li);
        existing = li;
    }

    setActivePrivateUser(existing);
}

// === Chọn user trong Chat Riêng ===
function setActivePrivateUser(element) {
    document.querySelectorAll("#privateChatUserList li").forEach(li => li.classList.remove("active"));
    element.classList.add("active");
    activePrivateUserId = element.getAttribute("data-user-id");
    loadPrivateMessages(activePrivateUserId);

    const input = document.getElementById("privateChatInput");
    input.focus();
}

// === Helper ===
function getUserAvatar(userId) {
    const li = document.querySelector(`#privateChatUserList li[data-user-id="${userId}"] img`);
    return li ? li.src : "/uploads/profile/default-img.jpg";
}
function getUserName(userId) {
    const li = document.querySelector(`#privateChatUserList li[data-user-id="${userId}"]`);
    return li ? li.getAttribute("data-user-name") : "Unknown";
}
function escapeHtml(text) {
    const div = document.createElement("div");
    div.innerText = text;
    return div.innerHTML;
}

// === Binding events ===
document.addEventListener("DOMContentLoaded", function () {
    initPrivateChat();

    document.getElementById("sendPrivateChatBtn")?.addEventListener("click", sendPrivateMessage);
    document.getElementById("privateChatInput")?.addEventListener("keypress", function (e) {
        if (e.key === "Enter") sendPrivateMessage();
    });

    document.querySelectorAll(".start-private-chat").forEach(btn => {
        btn.addEventListener("click", function () {
            if (this.tagName === "BUTTON") {
                handleStartPrivateChat(this);
            } else {
                setActivePrivateUser(this);
            }
        });
    });
});
