![GitHub all releases](https://img.shields.io/github/downloads/Nothing-Just-a-Code/AuraJDB/total?style=for-the-badge)


# AuraJDB

**AuraJDB** is a high-performance, lightweight JSON-based database solution for **VB.NET** applications. Designed for local storage scenarios, it allows developers to persist structured data without the complexity of traditional database engines.

AuraJDB is ideal for applications that need fast, reliable, file-based storage using familiar VB.NET data types such as `Dictionary` and `List`.

---

## 🔧 Features

- 🧾 **File-Based JSON Storage**  
  Simple and human-readable data persistence.

- ⚡ **In-Memory Access**  
  Optimized for speed by caching data in memory with optional periodic syncing.

- 🧱 **Key-Value Data Model**  
  Access and manipulate objects through straightforward key-value logic.

- 🧩 **AuraList and Dictionary Support**  
  Compatible with custom types and nested structures, including support for AuraList.

- ✅ **No External Dependencies**  
  Only depends on `Newtonsoft.Json`. No database servers or engines required.

---

## 📦 Installation

Install via **NuGet Package Manager**

```powershell
Install-Package AuraJDB

## 🚀 **Quick Start**
1. Import the Namespace
<pre lang="markdown"> ```vbnet Imports AuraJDB
 ``` </pre>

2. Initialize the Database
<pre lang="markdown"> ```vbnet Dim db As New AuraJDB("data.json")
 ``` </pre>

3. Write Data
<pre lang="markdown"> ```vbnet db.Write("user123", New With {
    .username = "zeus",
    .level = 42,
    .roles = New List(Of String) From {"admin", "mod"}
})
 ``` </pre>

4. Read Data
<pre lang="markdown"> ```vbnet Dim user = db.GetSingleItem(dbpath, "key")
 ``` </pre>
