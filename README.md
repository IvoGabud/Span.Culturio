# Span.Culturio API

## Opis Projekta

Culturio je monolitna aplikacija koja povezuje kulturne ustanove (muzeji, galerije, kazališta) s korisnicima koji žele platiti mjesečnu pretplatu kako bi mogli posjetiti kulturne ustanove po povoljnijim cijenama.

Projekt je implementiran kao REST API s Entity Framework Core integracijom za upravljanje bazom podataka i JWT autentifikacijom za sigurnost.

## Pokretanje Projekta

### Preduvjeti

- .NET 8 SDK
- SQL Server

### Instalacija

1. Klonirajte repozitorij

```bash
git clone https://github.com/IvoGabud/Span.Culturio.git
cd Span.Culturio
```

2. Kreirajte `secrets.json` datoteku s potrebnom konfiguracijom prema sljedećem predlošku:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=Culturio;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "Secret": "6f47f0b3c2efa549bfdd7b1e6bed694a"
  }
}
```

3. Kreirajte bazu podataka

```bash
cd Span.Culturio.Api
dotnet ef database update
```

Baza će se automatski popuniti s testnim podatcima:

- **Testni korisnik**: username: `admin`, password: `Admin123!`
- **3 paketa**: Osnovni paket (30 dana), Premium paket (90 dana), Godišnji paket (365 dana)
- **3 kulturna objekta**: Muzej Mimara, Hrvatsko narodno kazalište, Muzej suvremene umjetnosti

4. Pokrenite aplikaciju

```bash
dotnet run
```

5. Otvorite Swagger dokumentaciju

```
https://localhost:7182/swagger
```

## API Endpoints - Upute za Testiranje

### 1. AUTH

#### POST `/auth/register` - Registracija novog korisnika

```json
{
  "firstName": "Test",
  "lastName": "User",
  "email": "test.user@example.com",
  "username": "testuser",
  "password": "TestPassword123!"
}
```

#### POST `/auth/login` - Prijava korisnika

```json
{
  "username": "testuser",
  "password": "TestPassword123!"
}
```

**Odgovor**: Vraća JWT token koji se koristi za autentifikaciju kod ostalih endpointa

---

### 2. USERS (Zahtijeva JWT Token)

#### GET `/users?pageSize=10&pageIndex=0` - Dohvati popis korisnika

**Query parametri:**

- `pageSize`: `10`
- `pageIndex`: `0`

**Headers:**

```
Authorization: Bearer {vaš-jwt-token}
```

#### GET `/users/{id}` - Dohvati korisnika po ID-u

**Primjer:** `/users/1`

**Headers:**

```
Authorization: Bearer {vaš-jwt-token}
```

---

### 3. CULTURE OBJECTS (Zahtijeva JWT Token)

#### POST `/culture-objects` - Kreiraj novi kulturni objekt

```json
{
  "name": "Muzej Mimara",
  "contactEmail": "kontakt@mimara.hr",
  "address": "Rooseveltov trg 5",
  "zipCode": 10000,
  "city": "Zagreb",
  "adminUserId": 1
}
```

**Headers:**

```
Authorization: Bearer {vaš-jwt-token}
```

#### GET `/culture-objects` - Dohvati sve kulturne objekte

**Headers:**

```
Authorization: Bearer {vaš-jwt-token}
```

#### GET `/culture-objects/{id}` - Dohvati kulturni objekt po ID-u

**Primjer:** `/culture-objects/1`

**Headers:**

```
Authorization: Bearer {vaš-jwt-token}
```

---

### 4. PACKAGES (Zahtijeva JWT Token)

#### GET `/packages` - Dohvati sve pakete s kulturnim objektima

**Headers:**

```
Authorization: Bearer {vaš-jwt-token}
```

U bazi su već stvoreni primjeri paketa budući da endpoint za stvaranje nije specificiran.

---

### 5. SUBSCRIPTIONS (Zahtijeva JWT Token)

#### POST `/subscriptions` - Kreiraj novu pretplatu

```json
{
  "userId": 1,
  "packageId": 1,
  "name": "Godišnja muzejska kartica"
}
```

**Headers:**

```
Authorization: Bearer {vaš-jwt-token}
```

#### GET `/subscriptions?userId=1` - Dohvati pretplate

**Query parametri (opcionalno):**

- `userId`: `1` (filtriraj po korisniku)

**Headers:**

```
Authorization: Bearer {vaš-jwt-token}
```

#### POST `/subscriptions/activate` - Aktiviraj pretplatu

```json
{
  "subscriptionId": 1
}
```

**Headers:**

```
Authorization: Bearer {vaš-jwt-token}
```

#### POST `/subscriptions/track-visit` - Zabilježi posjetu kulturnom objektu

```json
{
  "subscriptionId": 1,
  "cultureObjectId": 1
}
```

**Headers:**

```
Authorization: Bearer {vaš-jwt-token}
```

---
