A.

1. đã clone repo rồi -> chuyển sang nhánh riêng và pull:

git fetch origin (so sánh code)
   
git branch -r (để check branch trong repo)

2. Tạo nhánh local và chuyển sang nhánh đó
Giả sử nhánh bạn cần là hoang/project-demo, chạy:

git checkout -b hoang/project-demo origin/hoang/project-demo (vừa tạo branch vừa remote chính branch mới tạo)

3. Pull source về từ remote (lần sau),
Khi đang ở đúng nhánh:

git pull (Lưu ý: pull khi đang ở nhánh mà mình muốn lấy về)

B. 
1.  chưa clone repo bao giờ -> Clone toàn bộ repository: git clone https://github.com/LuongFu/SWRT_D11-RT01_Nihongo_Sekai.git

2.  Xem các nhánh có sẵn:

git branch -r (view branch đang tồn tại trong repo)

3.  Checkout nhánh muốn làm việc:

Example: git checkout -b hoang/project-demo origin/hoang/project-demo

C.

Nếu bạn đang ở nhánh hoang/project-demo và muốn lấy (pull) thay đổi từ nhánh main về, tức là muốn cập nhật code mới từ main vào hoang/project-demo, bạn làm như sau:

✅ 1. Đảm bảo bạn đang ở hoang/project-demo

git branch

Nếu chưa, hãy chuyển:

example: git checkout hoang/project-demo

✅ 2. Lấy dữ liệu mới nhất từ remote (không gộp vào nhánh hiện tại)

git fetch origin

✅ 3. Merge nhánh main vào hoang/project-demo

git merge origin/main (có thể thay merge thành rebase)

4. Nếu bạn muốn push lại nhánh hoang/project-demo sau khi merge/rebase:

example: git push origin hoang/project-demo

D.
Nếu bạn muốn lấy code từ một nhánh phụ khác (không phải main) vào nhánh hiện tại, bạn chỉ cần thay thế tên nhánh là được. Dưới đây là hướng dẫn đầy đủ:

🎯 Giả sử:
Bạn đang ở nhánh feature/chatbot

Bạn muốn lấy code từ nhánh active/project-demo về để cập nhật

✅ Bước 1: Đảm bảo bạn đang ở đúng nhánh hiện tại

git checkout feature/chatbot

✅ Bước 2: Fetch toàn bộ dữ liệu từ remote

git fetch origin

✅ Bước 3: Merge nhánh phụ vào nhánh hiện tại

git merge origin/active/project-demo

Hoặc nếu muốn giữ lịch sử gọn hơn (khuyên dùng nếu làm việc cá nhân):

git rebase origin/active/project-demo

✅ Bước 4: Resolve conflict (nếu có), sau đó:

git add .

git commit -m "Resolve conflicts with active/project-demo"

# Nếu dùng rebase:
git rebase --continue

✅ Bước 5: Push thay đổi (nếu muốn)

git push origin feature/chatbot

(QUAN TRỌNG!)
| Lệnh        | Mục đích chính                                | Khi nào dùng                                     |
| ----------- | --------------------------------------------- | ------------------------------------------------ |
| `git clone` | Tải toàn bộ repo lần đầu từ remote            | Khi **bắt đầu làm việc với một project Git mới** |
| `git fetch` | Lấy dữ liệu mới từ remote **nhưng không gộp** | Khi muốn **xem trước** thay đổi của người khác   |
| `git pull`  | Lấy dữ liệu mới và **tự động gộp (merge)**    | Khi muốn **cập nhật nhanh code mới về máy mình** |

