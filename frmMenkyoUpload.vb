﻿Public Class frmMenkyoUpload
    Private Sub frmMenkyoUpload_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: このコード行はデータを 'PhoneNumDBDataSet.tbl_staff' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
        Me.Tbl_staffTableAdapter.Fill(Me.PhoneNumDBDataSet.tbl_staff)

    End Sub

    Private Sub btnFileSelect_Click(sender As Object, e As EventArgs) Handles btnFileSelect.Click
        'OpenFileDialogクラスのインスタンスを作成
        Dim ofd As New OpenFileDialog()

        'はじめのファイル名を指定する
        'はじめに「ファイル名」で表示される文字列を指定する
        ofd.FileName = "XXXX.pdf"
        'はじめに表示されるフォルダを指定する
        '指定しない（空の文字列）の時は、現在のディレクトリが表示される
        ofd.InitialDirectory = "\\192.168.8.190\share\★★個人用フォルダ★★\菅原\scan"
        '[ファイルの種類]に表示される選択肢を指定する
        '指定しないとすべてのファイルが表示される
        ofd.Filter = "pdfファイル(*.pdf)|*.pdf|すべてのファイル(*.*)|*.*"
        '[ファイルの種類]ではじめに選択されるものを指定する
        '2番目の「すべてのファイル」が選択されているようにする
        ofd.FilterIndex = 1
        'タイトルを設定する
        ofd.Title = "免許証ファイルを選択してください"
        'ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
        ofd.RestoreDirectory = True
        '存在しないファイルの名前が指定されたとき警告を表示する
        'デフォルトでTrueなので指定する必要はない
        ofd.CheckFileExists = True
        '存在しないパスが指定されたとき警告を表示する
        'デフォルトでTrueなので指定する必要はない
        ofd.CheckPathExists = True

        'ダイアログを表示する
        If ofd.ShowDialog() = DialogResult.OK Then
            'OKボタンがクリックされたとき、選択されたファイル名を表示する
            txtFilePath.Text = ofd.FileName.ToString
        End If

    End Sub

    Private Sub frmSyakenUpload_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: このコード行はデータを 'PhoneNumDBDataSet.tbl_car' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
        Me.Tbl_staffTableAdapter.Fill(Me.PhoneNumDBDataSet.tbl_staff)


    End Sub

    'アップロードボタン
    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        'ファイルが選択されていなければキャンセル
        If txtFilePath.Text = "" Then
            MsgBox("ファイルを選択してください")
            Return
        End If

        '選択された氏名の車検証をoldフォルダに移動
        Dim files As System.Collections.ObjectModel.ReadOnlyCollection(Of String) =
                 My.Computer.FileSystem.GetFiles(
                 "\\192.168.8.190\share\system\menkyo",
                  FileIO.SearchOption.SearchTopLevelOnly,
                 "" & cmbStaff.Text & "_*")

        '\\system\syakenに該当車番ファイルが見つからなかったら、メッセージを出して次へ
        If files.Count = 0 Then

            If MsgBox("氏名：" & cmbStaff.Text & "の以前の免許証ファイルが見つかりません。ファイルは新規に保存されます。続行しますか？", vbYesNo) = vbYes Then

            Else
                Return

            End If

            '\\system\menkyoに該当免許ファイルが見つかったら、以前の車検証ファイルはoldフォルダに移動する

        Else

            If MsgBox("氏名：" & cmbStaff.Text & "の以前の免許証ファイルが見つかりました。ファイルは上書きされます。続行しますか？", vbYesNo) = vbYes Then
                For Each f As String In files
                    System.IO.File.Move(f, "\\192.168.8.190\share\system\menkyo\old\" & Split(f, "\").Last)
                Next
            Else
                Return

            End If
        End If

        '選択された社員の新しい免許証をmenkyoフォルダにコピー＆リネーム　例：岩田　久彌_免許証_20170101.pdf
        Dim newMenkyoFilePath As String
        newMenkyoFilePath = "\\192.168.8.190\share\system\menkyo\" & cmbStaff.Text & "_免許証_" & dtpMenkyoLimit.Text.ToString & ".pdf"

        System.IO.File.Copy(txtFilePath.Text, newMenkyoFilePath)

        Me.Close()
    End Sub
End Class