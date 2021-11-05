# Jak uruchomić projekt?
### Utworzenie Azure Blob Storage 
1. Zaloguj się do Azure Portal
2. Przejdź do Storage accounts i utwórz nowy storage.
3. Przejdź do nowo utworzonego Storage i kliknij w menu na **Resource sharing (CORS)**.
4. Tam dodaj nową pozycję
![Ustawienia CORS!](https://soloprogramista.pl/wp-content/uploads/2021/11/BlobStorage-CORS-1024x482.png "Ustawienia CORS")
5. Następnie przejdź do sekcji **Access Keys** i znajdziesz tam connection stringa do swojego Storage. Skopiuj go i wpisz do pliku **appsettings.json**

### Utworzenie bazy danych
1. Stwój czystą bazę w Sql Server
2. Uruchom na tej bazie ten skrypt: https://github.com/nhibernate/NHibernate.AspNetCore.Identity/blob/master/database/mssql/00_aspnet_core_identity.sql
3. W projekcie **NoteBookApp.Server** znajdziesz w katalogu **Infrastructure** plik **DbCreate.sql**. Uruchom go na swojej bazie.
4. Podaj connection stringa do bazy w pliku **appsettings.json**
