# GranTorismo
Proyecto bases 2

# MongoDB
COMANDOS IMPORTANTES MONGO

--- Iniciar la base desde el cmd

"C:\Program Files\MongoDB\Server\3.4\bin\mongod.exe"

--- Agregar esto al PATH para agilizar el prender el server y conexiones

C:\Program Files\MongoDB\Server\3.4\bin

--- Iniciar el server

Ir al cmd y escribir mongod

--- Iniciar conexión

Ir a OTRO cmd y escribir mongo

--- Comandos para usar MongoDB en Python

import pymongo

from pymongo import MongoClient

client = MongoClient()

client = MongoClient('localhost', 27017)
