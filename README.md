 1. Cơ bản – Làm quen với Git & GitHub
1.1. Cài đặt & cấu hình lần đầu

git config --global user.name "Your Name"
git config --global user.email "you@example.com"
– Đây là thông tin cá nhân được ghi trong mọi commit bạn tạo.

1.2. Khởi tạo repository mới
Tạo local repo từ đầu:

git init
Hoặc clone từ trên GitHub:

git clone https://github.com/username/repo.git
1.3. Thao tác thường ngày
Kiểm tra trạng thái:

git status
Thêm thay đổi:

git add file1 file2 
git add .
Commit:

git commit -m "Mô tả ngắn gọn"
Xem lịch sử commit:

bash
Sao chép
Chỉnh sửa
git log --oneline
💡 Lưu ý:
Always viết commit message rõ ràng, có dấu để dễ đọc.

Chia nhỏ và gộp commit có logic để sau này dễ hiểu.

 2. Làm việc với repository trên GitHub
2.1. Push/Pull từ remote
push:

git push origin main
pull (kết hợp fetch + merge):

git pull origin main
fetch (chỉ đồng bộ metadata):

git fetch
git branch -r  # xem branch trên remote
2.2. Quản lý nhánh (Branch)
Tạo branch mới:

git checkout -b feature-x
Chuyển branch:

git checkout main
Push branch và thiết lập upstream:

git push -u origin feature-x
3. Thực chiến với Pull Request & Team workflow
Tạo PR trên GitHub từ feature‑x → main.

Người khác sẽ review, comment.

Sau khi được duyệt, merge vào main.

Trên máy local, bạn chuyển về main:

git checkout main
git pull origin main
🎯 Đây là quá trình phổ biến khi làm nhóm hoặc làm open‑source.

🧰 4. Mạnh hơn – Kỹ thuật nâng cao
4.1. Rebase
Giúp giữ lịch sử commit gọn, “dẹp” hơn:

git pull --rebase
4.2. Squash commit
Gộp nhiều commit vụn thành 1 để lịch sử sạch hơn.

4.3. Chặn push có file nhạy cảm
Sử dụng .gitignore, git filter-repo, hoặc GitHub Secrets…

Xử lý bằng cách:

git filter-repo --path secret.txt --invert-paths --force

(QUAN TRỌNG!)
| Lệnh        | Mục đích chính                                | Khi nào dùng                                     |
| ----------- | --------------------------------------------- | ------------------------------------------------ |
| `git clone` | Tải toàn bộ repo lần đầu từ remote            | Khi **bắt đầu làm việc với một project Git mới** |
| `git fetch` | Lấy dữ liệu mới từ remote **nhưng không gộp** | Khi muốn **xem trước** thay đổi của người khác   |
| `git pull`  | Lấy dữ liệu mới và **tự động gộp (merge)**    | Khi muốn **cập nhật nhanh code mới về máy mình** |

