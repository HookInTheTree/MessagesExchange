Данный проект представляет собой простой web-сервис обмена сообщениями.

Сервис состоит из трех компонентов:
- Три клиента (в данной реализации три разных страницы)
- API сообщений
- База данных

Функциональные требования:
- Первый клиент отправляет произвольные сообщения (по контенту) в сервис (на одно сообщение один вызов к API)
- Второй клиент отображает все новые (с момента подключения) сообщения в Real-Time. Необходимо выводить их в порядке прихода с сервера (с отображением метки времени и порядкового номера)
- Третий клиент отобразить историю сообщений за последние N (10 by default) минут
Каждое сообщение состоит из текста до 128 символов, метки даты/времени (устанавливается на сервере) и порядкового номера (приходит от клиента).

Нефункциональные требования:
- API сообщений придерживается архитектурного стиля REST.
- API сообщений содержит 2 метода:
  - отправить одно сообщение
  - получить список сообщений за диапазон дат

Желательно: сгенерить swagger-документацию (для REST-api).

Архитектурные требования: 
* MVC или подобная
* Слой DAL без использования ORM
* Ведение логов, чтобы по ним можно было понять текущее состояние работы приложения

Приложение нужно оформить в виде docker-образов.
Оформить docker-compose файл, при запуске которого стартуют все компоненты системы
