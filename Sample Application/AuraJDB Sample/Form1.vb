Imports AuraJDB

Public Class Form1
    Private ADB As AuraJDB.AuraJDB
    Public players As New AuraDataPath()
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ADB = New AuraJDB.AuraJDB(IO.Path.Combine(Application.StartupPath, "my database"))
        players = New AuraDataPath().AddPath("players")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            ADB.Write(players, "id", TextBox1.Text)
            ADB.Write(players, "name", TextBox2.Text)
            ADB.Write(players, "points", TextBox3.Text)
            Dim nts As New AuraList
            For Each item In TextBox4.Text.Split(New Char() {"$"c})
                nts.Add(item)
            Next
            ADB.Write(players, "notes", nts)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim player As AuraObject = ADB.Search("id", TextBox8.Text)
        If player Is Nothing Then
            MsgBox("Player not found")
        Else
            TextBox7.Text = player("name").ToString()
            TextBox6.Text = player("points").ToString()
            Dim notes As AuraList = player.GetValue(Of AuraList)(player("notes"))
            TextBox5.Text = String.Join("$"c, notes)
        End If
    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click
        Dim item = ADB.GetSingleItem(players, "id")
        MsgBox(item)
    End Sub
End Class
