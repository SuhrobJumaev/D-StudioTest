 ## Установка
 - Cклонируйте репозиторий: git clone https://github.com/SuhrobJumaev/D-StudioTest.git
 - Перейдите в директорию проекта: cd /src
 - В дириектории Movies: выполнить команду docker-compose up --build
 - Подождать пока проект забилдится и запуститься в контейнере.
 - **Важно:** При первом запуске, после закачки всех образов докера, нужно подождать 1 или 2 минуты, пока запуститься контейнер с БД. После запуска БД, приложения автоматически создаст таблицы и создаст пользователя.
 - Приложения будет доступно по адресу: http://localhost:8081/swagger/index.html
 - Пользователь для авторизации: **Email:** admin@gmail.com **Password:** 123456
