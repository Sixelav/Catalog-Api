# REST API Catalog

Ce projet est une démonstation de compétence d'API REST.

L'application entière est contenue dans le dossier `Catalog.Api`.

`Catalog.UnitTests` est le projet de tests unitaires.

`Catalog.sln` contient les deux projets et peut être lancé avec Microsoft Visual Studio.

# Choix de coding

J'ai choisi de développer cette application sous la forme REST car cela donne une grande fléxibilité pour des ajouts futurs. En effet, la simplicité de la structure rend facile et accessible à de nombreux développeurs, l'ajout de fonctionnalité.

# Installation

Tout d'abord, il faudra chargé la solution dans Visual Studio (ou Visual Studio Code) en utilisant le fichier `Catalog.sln`. Cela ouvrira les deux projets, l'un contenant l'API et l'autres les tests unitaires.

Ensuite, il sera nécessaire de mettre en place une base de donnée contenant une table `Items` qui comportera les champs suivants:
###
    ID => int
    Name => nvarchar(50)
    ValidityStart => datetime
    ValidityEnd => datetime

Une fois la DB opérationnelle, il faudra ajouter la connection string dans le fichier `CatalogDBContext.cs` dans `OnConfiguring` en paramètre de la fonction `UseSqlServer`.

Une fois toutes ces étapes faites, il ne reste plus qu'à lancer le build dans Visual Studio afin de tester le projet! Les routes sont décrites ci-dessous.

# REST API

## Avoir une liste d'Items

### Requête

`GET /items/`

### Paramètres

Aucuns paramètres

### Réponse

    200 Ok

    content-type: application/json; charset=utf-8 
    date: Mon02 Aug 2021 23:05:04 GMT 
    server: Kestrel 

    [{"id":6,"name":"Lampe de chevet","validityStart":"2021-07-27T16:46:03.697","validityEnd":"2028-07-27T16:46:03.697"},...]

## Créer un nouvel Item

### Requête

`POST /items/`

### Paramètres

Aucuns paramètres

### Corps de requête

    {
      "name": "Lavabo",
      "validityStart": "2021-08-02T23:08:12.335Z",
      "validityEnd": "2025-08-02T23:08:12.335Z"
    }

### Réponse

    200 Ok

    content-type: application/json; charset=utf-8 
    date: Mon02 Aug 2021 23:09:22 GMT 
    server: Kestrel 

    {"id": 14,"name": "Lavabo", "validityStart": "2021-08-02T23:08:12.335Z", "validityEnd": "2025-08-02T23:08:12.335Z"}

## Avoir un Item spécifique

### Requête

`GET /items/{id}`

### Paramètres

    id


### Réponse

    200 Ok

    content-type: application/json; charset=utf-8 
    date: Mon02 Aug 2021 23:14:18 GMT 
    server: Kestrel 

    {"id": 14,"name": "Lavabo", "validityStart": "2021-08-02T23:08:12.335Z", "validityEnd": "2025-08-02T23:08:12.335Z"}


## Supprimer un Item

### Requête

`DELETE /items/{id}`

### Paramètres

    id

### Réponse
    
    204 No Content

    date: Mon02 Aug 2021 23:17:52 GMT 
    server: Kestrel

