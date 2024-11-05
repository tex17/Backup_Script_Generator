# Генератор .bat-скриптов для резервного копирования
Генератор формирует скрипт по заданным параметрам. Предполагается, что созданный скрипт будет занесён в планировщик задач Windows, для переодического создания бэкапов.

# Принцип работы
Алгоритм скрипта сравнивает перечень файлов исходной папки и всех вложенных файлов в папке бэкапов. При наличии новых файлов, которые не содержатся в папке с бэкапами -- создаёт подпапку с текущей датой и помещает в неё новые файлы. (При повторном запуске в ту же дату -- сохраняет в уже созданную папку.)

# Состояние проекта
На данный момент, генератор позволяет указать лишь пути к исходным данным и месту бэкапа. Но не позволяет указать необходимые расширения для файлов. (Жёстко указаны .jpg, .jpeg, .bmp, .raw и .dcm)
