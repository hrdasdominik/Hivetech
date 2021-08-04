Postovani,

htio bih prvo napomenut, da bi aplikacija radila treba se ucinit tri stvari.

Prvo:
koristio sam SQL Server Management Studio (SSMS) te sam ostavio bazu pod folderom "Database" s nazivom "HivetechDb.mdf" i "HivetechDb_log.ldf" koju bi trebalo integrirati.

Drugo:
kada se otvori sami solution aplikacije u datoteci "appsettings.json" treba izmjenit "ConectionStrings" - "EmployeeContext" gdje treba staviti svoj link data source-a integrirane baze koju sam Vam ostavio.

Trece:
podaci za ulogiravanje su sljeci:

za administratora:
Username: pmasic
Password: 1234

za employee-a generalno ide inicijal imena + prezime i lozinka isto 1234
Npr:
Username: ssatir
Password: 1234

Nadam se da Vam to neće predstavljati problem.

Srdacan pozdrav,
Dominik Hrdas