Public Class Form1
    
    
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load


    End Sub

    Private Function DrawLineFloat(ByVal x1 As Single, ByVal y1 As Single, ByVal x2 As Single, ByVal y2 As Single, ByVal thickness As Integer)
        ' Create pen. 
        Try
            Dim blackPen As New Pen(Color.LightBlue, thickness)
            ' Create coordinates of points that define line. 

            ' Draw line to screen. 
            Dim g As Graphics = Me.CreateGraphics()

            g.DrawLine(blackPen, g.VisibleClipBounds.Right + x1, g.VisibleClipBounds.Bottom - y1, g.VisibleClipBounds.Right + x2, g.VisibleClipBounds.Bottom - y2)

            g.Dispose()
            blackPen.Dispose()
            Return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)) / 10
        Catch ex As Exception

        End Try


    End Function
    Private Function CleaR()
        Me.Refresh()
        
    End Function
    Private Sub Button1_Click(sender As Object, e As EventArgs)
    End Sub

    

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        CleaR()
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown

        CleaR()
        Try
            TextBox1.Text = My.Computer.FileSystem.ReadAllText(My.Application.CommandLineArgs(0))
            Label4.Text = "Analysiere..."
            Me.Update()
            DrawData()
            Label4.Text = "Fertig."
            Button5.Enabled = True
        Catch ex As Exception

        End Try



    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            OpenFileDialog1.ShowDialog()
            TextBox1.Text = My.Computer.FileSystem.ReadAllText(OpenFileDialog1.FileName)
            Label4.Text = OpenFileDialog1.FileName
            Button5.Enabled = True
            Label7.Text = "Status: Nicht kompiliert"
        Catch ex As Exception

        End Try
        

    End Sub

    Private Function GetXYZ(ByVal Zeile As String)
        Dim res(2) As Single
        Dim start As Integer = 0
        Dim ende As Integer
        If Zeile.Contains("X") And Zeile.Contains("G92") = False Then

            While Zeile.Substring(start, 1) <> "X"
                start = start + 1
            End While
            ende = start
            While Zeile.Substring(ende, 1) <> "."
                ende = ende + 1
            End While
            res(0) = CSng(Zeile.Substring(start + 1, ende - (start - 1)).Replace(".", ""))
        Else
            res(0) = vbNull
        End If

        If Zeile.Contains("Y") And Zeile.Contains("G92") = False Then

            start = ende
            While Zeile.Substring(start, 1) <> "Y"
                start = start + 1
            End While
            ende = start
            While Zeile.Substring(ende, 1) <> "."
                ende = ende + 1
            End While

            res(1) = CSng(Zeile.Substring(start + 1, ende - (start - 1)).Replace(".", ""))

        Else
            res(1) = vbNull
        End If

        If Zeile.Contains("Z") And Zeile.Contains("G92") = False Then

            start = ende
            While Zeile.Substring(start, 1) <> "Z"
                start = start + 1
            End While
            ende = start
            While Zeile.Substring(ende, 1) <> "."
                ende = ende + 1
            End While

            res(2) = CSng(Zeile.Substring(start + 1, ende - (start - 1)).Replace(".", ""))

        Else
            res(2) = vbNull
        End If
        Return res

    End Function

    

    

    Private Function DrawData()
        Dim a As Long
        Dim PrevX As Single = 0
        Dim PrevY As Single = 0
        Dim PrevZ As Single = vbNull
        Dim c, d As Single
        Dim MaxX As Single = 0
        Dim MaxY As Single = 0
        Dim lenght As Double = 0
        Dim res(2) As Single
        Dim z As Single
        For a = 0 To TextBox1.Lines.LongCount - 1
            res = GetXYZ(TextBox1.Lines(a))
            c = res(0)
            d = res(1)
            z = res(2)
            If z = vbNull And PrevZ = vbNull Then
                PrevZ = 1
            ElseIf z = vbNull And PrevZ < 0 Then
                PrevZ = PrevZ
            ElseIf z = vbNull And PrevZ > 0 Then
                PrevZ = PrevZ
            Else
                PrevZ = z
            End If

            If c = vbNull And d = vbNull Then

            ElseIf c = vbNull Then
                If PrevZ < 0 Then lenght = lenght + DrawLineFloat(PrevX, PrevY, PrevX, d, 1)

                PrevY = d
                If PrevY > MaxY Then MaxY = PrevY
                If PrevX < MaxX Then MaxX = PrevX
            ElseIf d = vbNull Then
                If PrevZ < 0 Then lenght = lenght + DrawLineFloat(PrevX, PrevY, c, PrevY, 1)
                PrevX = c

                If PrevY > MaxY Then MaxY = PrevY
                If PrevX < MaxX Then MaxX = PrevX
            Else
                If PrevZ < 0 Then lenght = lenght + DrawLineFloat(PrevX, PrevY, c, d, 1)
                PrevX = c
                PrevY = d
                If PrevY > MaxY Then MaxY = PrevY
                If PrevX < MaxX Then MaxX = PrevX
            End If

            Label6.Text = Str(Int((a / (TextBox1.Lines.LongCount - 1)) * 100)) + "%"
            Me.Update()
            Application.DoEvents()
        Next
        Label2.Text = "Höhe: " + Str(MaxY / 10) + "mm"
        Label3.Text = "Breite: " + Str(MaxX / -10) + "mm"
        Label8.Text = "Fräsweg: " + Str(Int(lenght)) + "mm"
        Return 1
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Label4.Text = "Analysiere..."
        Me.Update()
        DrawData()
        Label4.Text = "Fertig."
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Label4.Text = "Quick Scan..."
        Me.Update()
        Dim a As Long
        Dim PrevX As Single = 0
        Dim PrevY As Single = 0
        Dim MaxX As Single = 0
        Dim MaxY As Single = 0
        Dim res(2) As Single
        For a = 0 To TextBox1.Lines.LongCount - 1
            Try
                res = GetXYZ(TextBox1.Lines(a))
                PrevX = res(0)
                PrevY = res(1)
                If PrevY > MaxY Then MaxY = PrevY
                If PrevX < MaxX Then MaxX = PrevX
            Catch ex As Exception

            End Try

                

            Label6.Text = Str(Int((a / (TextBox1.Lines.LongCount - 1)) * 100)) + "%"
            Me.Update()
            Application.DoEvents()
        Next
        Label2.Text = "Höhe: " + Str(MaxY / 10) + "mm"
        Label3.Text = "Breite: " + Str(MaxX / -10) + "mm"
        Label4.Text = "Fertig."
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        CleaR()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        For a As Integer = 0 To TextBox1.Lines.Count - 1
            If TextBox1.Lines(a).Contains("(") Then
                TextBox1.Text = TextBox1.Text.Replace(TextBox1.Lines(a), "")
            End If
            If TextBox1.Lines(a).Contains("T") Then
                TextBox1.Text = TextBox1.Text.Replace(TextBox1.Lines(a), "")
            End If
            If TextBox1.Lines(a).Contains("M") Then
                TextBox1.Text = TextBox1.Text.Replace(TextBox1.Lines(a), "")
            End If
            If TextBox1.Lines(a).Contains("G04") Then
                TextBox1.Text = TextBox1.Text.Replace(TextBox1.Lines(a), "")
            End If
            Label7.Text = "Status: " + Str(Int((a / (TextBox1.Lines.LongCount - 1)) * 100)) + "% Kompiliert"
            Application.DoEvents()
        Next
        TextBox1.Text = TextBox1.Text.Replace("G01", "G1")
        TextBox1.Text = TextBox1.Text.Replace("G00", "G0")
        TextBox1.Text = "G90" + vbNewLine + "G21" + vbNewLine + "M3" + vbNewLine + "G92 X0.00 Y0.00 Z0.00" + TextBox1.Text.Replace("G90", "").Replace("G21", "") + vbNewLine + "G0 X0.00 Y0.00" + vbNewLine + "M5" + vbNewLine + "M84"
        Label7.Text = "Status: Speichern"
        SaveFileDialog1.ValidateNames = False
        SaveFileDialog1.AddExtension = True
        SaveFileDialog1.DefaultExt = ".gcode"
        SaveFileDialog1.FileName = ""
        SaveFileDialog1.ShowDialog()
        Try
            My.Computer.FileSystem.WriteAllText(SaveFileDialog1.FileName, TextBox1.Text, False)
            Label7.Text = "Status: Fertig"
        Catch ex As Exception

        End Try

    End Sub

    
End Class
