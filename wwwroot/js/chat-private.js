// ==================== CHAT RIÊNG ====================
let privateChatTargetId = null;
let privateConnection = null;

const privateChatBox = document.getElementById("privateChatMessages");
const privateChatInput = document.getElementById("privateChatInput");
const classroomId = window.classroomId;
const currentUserId = window.currentUserId;

// Escape text để chống XSS
function escapeHtml(text) {
    const div = document.createElement("div");
    div.appendChild(document.createTextNode(text));
    return div.innerHTML;
}

// Thêm tin nhắn vào UI
function appendPrivateMessage(user, avatar, message, sentAt, isOwn) {
    const sideClass = isOwn ? 'chat-bubble own' : 'chat-bubble';
    const timeHtml = sentAt ? `<small class="text-muted ms-2">(${sentAt})</small>` : '';
    const avatarSrc = avatar && avatar.trim() !== '' ? avatar : '/uploads/profile/default-img.jpg';

    privateChatBox.innerHTML += `
        <div class="${sideClass}" title="${escapeHtml(user)}">
            <img src="${avatarSrc}" alt="avatar" class="chat-avatar"/>
            <div class="chat-content">${escapeHtml(message)} ${timeHtml}</div>
        </div>`;
    privateChatBox.scrollTop = privateChatBox.scrollHeight;
}

// Load lịch sử tin nhắn
async function loadPrivateChatMessages(userId) {
    try {
        const res = await fetch(`/ClassroomInstances/GetPrivateChatMessages?classroomId=${classroomId}&targetUserId=${userId}`);
        if (res.ok) {
            const data = await res.json();
            privateChatBox.innerHTML = data.map(m => `
                <div class="${m.isOwn ? 'chat-bubble own' : 'chat-bubble'}" title="${escapeHtml(m.userName)}">
                    <img src="${m.avatarUrl}" alt="avatar" class="chat-avatar"/>
                    <div class="chat-content">
                        <span class="chat-message">${escapeHtml(m.message)}</span>
                        <small class="text-muted ms-2">(${m.sentAt})</small>
                    </div>
                </div>`).join('');
            privateChatBox.scrollTop = privateChatBox.scrollHeight;
        }
    } catch (err) {
        console.error("LoadPrivateChat error:", err);
    }
}

// Tạo kết nối SignalR
async function startPrivateConnection(targetUserId) {
    if (privateConnection) {
        await privateConnection.stop();
        privateConnection = null;
    }

    privateConnection = new signalR.HubConnectionBuilder()
        .withUrl(`/privateChatHub?classroomId=${classroomId}&targetUserId=${targetUserId}`)
        .build();

    privateConnection.on("ReceivePrivateMessage", (userName, message, sentAt, senderId, avatarUrl) => {
        const isOwn = senderId === currentUserId;
        appendPrivateMessage(userName, avatarUrl, message, sentAt, isOwn);
    });

    privateConnection.start()
        .then(() => console.log(`✅ Chat riêng - Connected with ${targetUserId}`))
        .catch(err => console.error("❌ PrivateChat connect error:", err));
}

// Gửi tin nhắn
function sendPrivateChatMessage() {
    const msg = privateChatInput.value.trim();
    if (!msg || !privateChatTargetId) return;

    privateConnection.invoke("SendPrivateMessage", classroomId, privateChatTargetId, msg)
        .then(() => privateChatInput.value = '')
        .catch(err => console.error("SendPrivateMessage error:", err));
}

// Event chọn user
$(document).on('click', '.start-private-chat', async function () {
    privateChatTargetId = $(this).data('user-id');
    $('#privateChatUserList .list-group-item').removeClass('active');
    $(this).addClass('active');
    await loadPrivateChatMessages(privateChatTargetId);
    await startPrivateConnection(privateChatTargetId);

    // Auto switch sang tab Chat Riêng
    const privateTab = document.querySelector('#private-chat-tab');
    if (privateTab) new bootstrap.Tab(privateTab).show();
});

// Event gửi tin nhắn
$('#sendPrivateChatBtn').on('click', sendPrivateChatMessage);
privateChatInput.addEventListener('keydown', e => {
    if (e.key === "Enter" && !e.shiftKey) {
        e.preventDefault();
        sendPrivateChatMessage();
    }
});

// Auto chọn user đầu tiên khi mở tab Chat Riêng
$('#private-chat-tab').on('shown.bs.tab', function () {
    const firstUser = $('#privateChatUserList .list-group-item').first();
    if (firstUser.length > 0) firstUser.trigger('click');
});
