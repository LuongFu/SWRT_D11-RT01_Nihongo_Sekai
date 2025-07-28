// ========== CHAT CHUNG ==========
const classroomId = window.classroomId;
const currentUserId = window.currentUserId;
const userName = window.currentUserName;

const chatBox = document.getElementById("chatMessages");
const chatInput = document.getElementById("chatInput");

// Escape HTML để chống XSS
function escapeHtml(text) {
    const div = document.createElement("div");
    div.appendChild(document.createTextNode(text));
    return div.innerHTML;
}

// Thêm tin nhắn vào UI
function appendMessage(user, avatar, message, sentAt, isOwn) {
    const sideClass = isOwn ? 'chat-bubble own' : 'chat-bubble';
    const timeHtml = sentAt ? `<small class="text-muted ms-2">(${sentAt})</small>` : '';
    const avatarSrc = avatar && avatar.trim() !== '' ? avatar : '/uploads/profile/default-img.jpg';

    chatBox.innerHTML += `
        <div class="${sideClass}" title="${escapeHtml(user)}">
            <img src="${avatarSrc}" alt="avatar" class="chat-avatar"/>
            <div class="chat-content">${escapeHtml(message)} ${timeHtml}</div>
        </div>`;
    chatBox.scrollTop = chatBox.scrollHeight;
}

// Load lịch sử tin nhắn
async function loadMessages() {
    try {
        const res = await fetch(`/ClassroomInstances/GetChatMessages?classroomId=${classroomId}`);
        if (res.ok) {
            const data = await res.json();
            chatBox.innerHTML = data.map(m => `
                <div class="${m.isOwn ? 'chat-bubble own' : 'chat-bubble'}" title="${m.userName}">
                    <img src="${m.avatarUrl}" alt="avatar" class="chat-avatar"/>
                    <div class="chat-content">
                        <span class="chat-message">${m.message}</span>
                        <small class="text-muted ms-2">(${m.sentAt})</small>
                    </div>
                </div>`).join('');
            chatBox.scrollTop = chatBox.scrollHeight;
        }
    } catch (err) {
        console.error("LoadMessages error:", err);
    }
}

// Kết nối SignalR
const connection = new signalR.HubConnectionBuilder()
    .withUrl(`/classroomChatHub?classroomId=${classroomId}`)
    .build();

connection.on("ReceiveMessage", (user, message, sentAt, senderId, avatarUrl) => {
    const isOwn = senderId === currentUserId;
    appendMessage(user, avatarUrl, message, sentAt, isOwn);
});

connection.start()
    .then(() => console.log("✅ Chat chung - SignalR connected"))
    .catch(err => console.error("❌ Chat chung - SignalR connect error:", err));

// Gửi tin nhắn
function sendMessage() {
    const message = chatInput.value.trim();
    if (!message) return;
    connection.invoke("SendMessage", classroomId, userName, message)
        .then(() => chatInput.value = '')
        .catch(err => console.error("❌ SendMessage error:", err));
}

// Event gửi khi nhấn Enter
chatInput.addEventListener("keydown", e => {
    if (e.key === "Enter" && !e.shiftKey) {
        e.preventDefault();
        sendMessage();
    }
});

// Load khi mở tab Chat
document.querySelector('#chat-tab')?.addEventListener('click', loadMessages);
