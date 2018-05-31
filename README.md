# OnlineSinavPortali


* PROJE GENEL YAPISI


- Kullanıcıların sisteme kayıt olup sınavlara bu platform üzerinden girdiği, sınavların bu platform üzerinden oluşturulup	 değerlendirildiği ve sonuçlarının bu platform üzerinden yayınlandığı bir web portalı geliştirilmiştir.

- Proje C# - .NET teknolojileri ile kodlanmıştır.

- Frontend kodlamada web teknolojileri (Html, Css, Javascript, Jquery) kullanılmıştır.

- Backend kısmında C#, ASP.NET MVC kullanılmıştır.

- Server hizmeti olarak IIS hizmeti kullanılmıştır.
 
- Veritabanı dili olarak T-SQL kullanılmıştır. Veritabanın ihtiyacı olan server ihtiyacı Microsoft Sql Server ile giderilmiştir.

<br>

- Portalda 3 adet görev tanımlı kişi vardır.

  - Kullanıcı
   
    a)	Kayıt olur.
    b)	Sınavlara girer.
    c)	Sınav istatistiklerini görür.
    d)	Sınavlara yorum yaparlar ve puan verirler.
    
  - Akademisyen
  
    a)	Kayıt olur.
    b)	İlgili sınavı oluşturup sınavı yayınlar.
    c)	İlgili sınav ile istatistikleri görür.
    d)	Sınav ile ilgili yorumları görür.
    
  - Değerlendirici
  
    a)	Kayıt olur.
    b)	Sınavları değerlendirir ve sonuçları yayınlar.
    c)	İlgili sınav ile istatistikleri paylaşır

<br>

- Portal Olay Akışı aşağıdaki gibidir:


  * Akademisyenler sınavları oluşturur ve yayınlarlar. 
  * Kullanıcılar ilgili  kategorilerde sınavlara girerler ve sınavları puanlayıp yorumlarlar. 
  * Değerlendiriciler sınav sonuçlarını ve istatistiklerini hesaplarlar.
  * Akademisyenler ilgili sınavlara yapılan yorumları ve ilgili sınav istatistiklerini görürler.
  * Kullanıcılar ilgili sınavlara yapılan yorumları ve ilgili sınav istatistiklerini görürler.



* PROJE ÇALIŞTIRILABİLİR HALE GETİRME ADIMLARI


 - Sisteminizde Visual Studio 2017, Microsoft Sql Server, Microsoft Sql Server Management Studio gibi yazılımların kurulu olması      gerekmektedir.
 
 - Proje ve proje ait ilgili database script dosyası "Clone or download" butonuna tıklanarak zip formatı halinde indirilir.
 
 - Zip dosyası içindeki dosyalar WinRAR ile boş bir klasöre çıkartılır.
 
 - Database script dosyası çalıştırılır ve açılan Sql Server Managemment Studio arayüzündeki "Execute" kısmına tıklanarak database oluşturulur.
 
 - Proje dosyalarının bulunduğu klasördeki OnlineSinavPortali.sln dosyası çalıştırılır ve proje Visual Studio da açılacaktır.
 
 - Proje kök dizin hiyerarşisindeki dosyalardan "portal.edmx" dosyası silinir.
 
 - Web.config dosyası altında "<connectionStrings> </connectionStrings>" arasındaki "<add name="SinavPortalEntities" şeklinde devam eden satır silinir.
     
 - Projeye sağ tıklanır. Ekle->Yeni Öğe->Veri->ADO.NET Entity Data Model yolu izlenir, Ad kısmından Data Modele bir isim verilir, Ekle 
 butonuna tıklanarak devam edilir.
 
 - Çıkan ekrandan EF Designer from database seçilir ve İleri ye tıklanarak devam edilir.
 
 - Çıkan ekrandan New Connection tıklanır. Sql Server sunucu adı girilir ve oluşturulan veritabanı seçilir. Tamam a tıklanarak devam edilir.
 
 - İleriye tıklanır ve mevcut databasedeki tablolar, saklı yordamlar, görünümler seçilir. Son a tıklanarak işlem sona erer. 
 
