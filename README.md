<h1 align="center" style="font-weight: bold;">AuraJDB</h1>


<p align="center">AuraJDB is a high-performance, lightweight JSON-based database solution for VB.NET applications. Designed for local storage scenarios, it allows developers to persist structured data without the complexity of traditional database engines.

AuraJDB is ideal for applications that need fast, reliable, file-based storage using familiar VB.NET data types such as Dictionary and List.</p>



<h2 id="technologies">💻 Features</h2>

    🧾 File-Based JSON Storage
    Simple and human-readable data persistence.

    ⚡ In-Memory Access
    Optimized for speed by caching data in memory with optional periodic syncing.

    🧱 Key-Value Data Model
    Access and manipulate objects through straightforward key-value logic.

    🧩 AuraList and Dictionary Support
    Compatible with custom types and nested structures, including support for AuraList.

    ✅ No External Dependencies
    Only depends on Newtonsoft.Json. No database servers or engines required.


<h3>Prerequisites</h3>

You need these libraries to use AuraJDB.

- [Newtonsoft.Json](https://www.newtonsoft.com/jsonschema)

<h3>Cloning</h3>

How to clone your project

```bash
git clone https://github.com/Nothing-Just-a-Code/AuraJDB.git
```
Or install from Package Manager Console

```bash
Install-Package AuraJDB
```

<h3>Starting</h3>

```bash
Imports AuraJDB
```

<h2 id="routes">📦 How to Use</h2>

<h3 id="get-auth-detail">Initialize the Database</h3>

**Code**
```vbnet
Dim db As New AuraJDB("data.json")
```

<h3 id="post-auth-detail">Write Data</h3>

**Code**
```vbnet
db.Write("user123", New With {
    .username = "zeus",
    .level = 42,
    .roles = New List(Of String) From {"admin", "mod"}
})
```


<h3 id="post-auth-detail">Read Data</h3>

**Code**
```vbnet
Dim user = db.GetSingleItem(dbpath, "key")
```
