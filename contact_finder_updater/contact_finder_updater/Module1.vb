Imports System.IO
Module Module1
    Sub Main()
        Dim CFProcesses() As Process
        Dim CDProcess As Process
        CFProcesses = Process.GetProcessesByName("Contact Finder")
        Try
            Do
                If CFProcesses.Length > 0 Then
                    For Each CDProcess In CFProcesses
                        If CDProcess IsNot Nothing Then CDProcess.Kill()
                    Next
                End If
                Console.WriteLine("Main app killed...")
            Loop Until CFProcesses.Length = 0
        Catch ex As Exception
        End Try

        Dim ChromeDriver_Processes() As Process
        Dim ChromeDriver_Process As Process
        ChromeDriver_Processes = Process.GetProcessesByName("chromedriver")
        Try
            If ChromeDriver_Processes.Length > 0 Then
                For Each ChromeDriver_Process In ChromeDriver_Processes
                    If ChromeDriver_Process IsNot Nothing Then ChromeDriver_Process.Kill()
                Next
            End If
            Console.WriteLine("Chrome driver killed...")
        Catch ex As Exception
        End Try

        Dim RootPath As String = Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\", "") ' Get executable directory where application is running from

        Dim UpdateFolder As String = RootPath & "\Update_Files"

        Dim AppFiles As String() = Directory.GetFiles(UpdateFolder, "*", SearchOption.AllDirectories)

        For Each file As String In AppFiles
            If Not file.Contains("contact_finder_updater") Then
                Console.WriteLine("Updating: " & file)
                Dim success As Boolean = False
                Do
                    Threading.Thread.Sleep(1000)
                    Try
                        My.Computer.FileSystem.CopyFile(file, file.Replace("\Update_Files", ""), overwrite:=True)
                        success = True
                    Catch ex As Exception
                    End Try
                Loop Until success = True
            End If
        Next

        DeleteDirectory(UpdateFolder)

        Console.WriteLine("Update completed. Press any key to quit.")

        Console.ReadKey(True)

        Dim MainAppPath As String = ""
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(RootPath, FileIO.SearchOption.SearchAllSubDirectories, "*.exe")
            If foundFile.Contains("Contact Finder") Then
                MainAppPath = foundFile
                Exit For
            End If
        Next
        Dim p As Process = Process.Start(MainAppPath)
    End Sub
    Public Sub DeleteDirectory(target_dir As String)
        Try
            Dim files As String() = Directory.GetFiles(target_dir)
            Dim dirs As String() = Directory.GetDirectories(target_dir)

            For Each fileInDir As String In files
                File.SetAttributes(fileInDir, FileAttributes.Normal)
                File.Delete(fileInDir)
            Next

            For Each dir As String In dirs
                DeleteDirectory(dir)
            Next

            Directory.Delete(target_dir, False)
        Catch ex As Exception
            Directory.Delete(target_dir, False)
        End Try
    End Sub
End Module
