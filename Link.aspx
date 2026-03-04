<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Link.aspx.cs" Inherits="ipsw.Link" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Redirecting to Unified Finder</title>
    <style>
        body {
            font-family: "Segoe UI", Arial, sans-serif;
            background: #f5f7fb;
            color: #13213a;
            margin: 0;
            min-height: 100vh;
            display: grid;
            place-items: center;
        }

        .card {
            background: #fff;
            border-radius: 12px;
            border: 1px solid #dbe3ee;
            max-width: 560px;
            padding: 24px;
            box-shadow: 0 10px 24px rgba(19, 33, 58, 0.08);
        }

        a {
            color: #0d6efd;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="card" role="status" aria-live="polite">
            <h1 style="margin-top:0; font-size:1.3rem;">Redirecting...</h1>
            <p>
                <asp:Label ID="lblRedirectMessage" runat="server" Text="Sending you to the unified firmware finder."></asp:Label>
            </p>
            <p>If you are not redirected automatically, open <a href="Default.aspx?result=links">this link</a>.</p>
        </div>
    </form>
</body>
</html>
