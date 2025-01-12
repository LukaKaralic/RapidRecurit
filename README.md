# RapidRecruit

Sistem za upravljanje zapošljavanjem i apliciranjem za posao.  
Razvijeno za ITS projekat.

## Demo

Aplikacija je dostupna na: [https://its.lukakaralic.com](https://its.lukakaralic.com)

## Tehnologije

- .NET 8
- Entity Framework Core (ORM)
- Entity Framework Core Identity (Autentifikacija)
- MS SQL Server
- Tailwind CSS
- Docker
- GitHub Actions

## Funkcionalnosti

### Za Poslodavce
- Kreiranje i upravljanje oglasima za posao
- Pregled prijava kandidata
- Komunikacija sa kandidatima kroz integrisani sistem za poruke
- Upravljanje statusom prijava

### Za Kandidate
- Pregled dostupnih poslova
- Apliciranje za posao sa mogućnošću upload-a cv-a
- Komunikacija sa poslodavcima

## Autentifikacija i Autorizacija

- Registracija korisnika (Poslodavac/Kandidat)
- Email verifikacija
- Upravljanje profilom
- Resetovanje lozinke
- Dvostruki sistem uloga kroz Entity Framework Identity. Poslodavac i Trazilac posla  

## Deployment

Deployment je automatizovan kroz GitHub Actions:
1. Push na master granu pokreće GitHub Actions workflow
2. Aplikacija se builduje i pakuje u Docker container
3. Novi Docker image se pushuje na container registry
4. Watchtower na serveru automatski detektuje i povlači novu verziju i po potrebi pokrece migracije

## Lokalno Pokretanje

### Preduslovi
- .NET 8 SDK
- MS SQL Server
- Docker (opciono)

### Koraci za pokretanje
1. Klonirajte repozitorijum
```bash
git clone git@github.com:LukaKaralic/RapidRecurit.git
```

2. Podesite konekcioni string u `appsettings.json`

3. Pokrenite migracije
```bash
dotnet ef database update
```

4. Pokrenite aplikaciju
```bash
dotnet run
```

## Konfiguracija

Glavne konfiguracije se nalaze u:
- `appsettings.json` - Osnovne konfiguracije i konekcioni stringovi
- `Dockerfile` - Docker konfiguracija
- `.github/workflows` - GitHub Actions konfiguracija

## DevOps

- Continuous Integration: GitHub Actions
- Containerization: Docker
- Deployment: Automatski kroz Watchtower

