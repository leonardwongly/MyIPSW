<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Link.aspx.cs" Inherits="MyIPSWMinimal.Link" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="" />
    <meta name="author" content="Leonard Wong, MyIPSW" />
    <title>MyIPSW</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous" />
    <script src="https://code.jquery.com/jquery-3.4.1.slim.min.js" integrity="sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js" integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js" integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6" crossorigin="anonymous"></script>
    <!--- Favicon --->
    <link rel="apple-touch-icon" sizes="57x57" href="favicon/apple-icon-57x57.png" />
    <link rel="apple-touch-icon" sizes="60x60" href="favicon/apple-icon-60x60.png" />
    <link rel="apple-touch-icon" sizes="72x72" href="favicon/apple-icon-72x72.png" />
    <link rel="apple-touch-icon" sizes="76x76" href="favicon/apple-icon-76x76.png" />
    <link rel="apple-touch-icon" sizes="114x114" href="favicon/apple-icon-114x114.png" />
    <link rel="apple-touch-icon" sizes="120x120" href="favicon/apple-icon-120x120.png" />
    <link rel="apple-touch-icon" sizes="144x144" href="favicon/apple-icon-144x144.png" />
    <link rel="apple-touch-icon" sizes="152x152" href="favicon/apple-icon-152x152.png" />
    <link rel="apple-touch-icon" sizes="180x180" href="favicon/apple-icon-180x180.png" />
    <link rel="icon" type="image/png" sizes="192x192" href="favicon/android-icon-192x192.png" />
    <link rel="icon" type="image/png" sizes="32x32" href="favicon/favicon-32x32.png" />
    <link rel="icon" type="image/png" sizes="96x96" href="favicon/favicon-96x96.png" />
    <link rel="icon" type="image/png" sizes="16x16" href="favicon/favicon-16x16.png" />
    <link rel="manifest" href="/manifest.json">
    <meta name="msapplication-TileColor" content="#ffffff">
    <meta name="msapplication-TileImage" content="/ms-icon-144x144.png">
    <meta name="theme-color" content="#ffffff">
</head>

 <nav class="navbar navbar-expand-lg navbar-light bg-dark">
    <a class="navbar-brand" href="Default.aspx" style="color: white;">MyIPSW</a>
    <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarNav">
        <ul class="navbar-nav">
            <li class="nav-item">
                <a class="nav-link" href="Default.aspx" style="color: white;">Home</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="Link.aspx" style="color: white;">Link</a>
            </li>
        </ul>
    </div>
</nav>

<body style="background-color: #f5f5f5">
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-sm" style="margin-top: 30px">
                    <h4>Step 1</h4>
                    <div class="row">
                        <asp:RadioButtonList ID="rblOptions" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblOptions_SelectedIndexChanged" CssClass="form-check">
                            <asp:ListItem>Official</asp:ListItem>
                            <asp:ListItem>OTA</asp:ListItem>
                            <asp:ListItem>Version</asp:ListItem>
                            <asp:ListItem>Version (OTA)</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <br />


                </div>

                <div class="col-sm" style="margin-top: 30px;">
                    <h4>
                        <asp:Label ID="lblStep2" runat="server" Text="Step 2"></asp:Label>
                    </h4>
                    <br />
                    <div class="row">
                        <asp:DropDownList ID="ddlVersion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlVersion_SelectedIndexChanged" CssClass="form-control">
                            <asp:ListItem Text="Select Version" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <br />
                    <div class="row">
                        <asp:DropDownList ID="ddlVersionOTA" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlVersionOTA_SelectedIndexChanged" CssClass="form-control">
                            <asp:ListItem Text="Select Version (OTA)" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <br />
                    <div class="row">
                        <asp:DropDownList ID="ddliPhone" runat="server" OnSelectedIndexChanged="ddliPhone_SelectedIndexChanged" AutoPostBack="True" CssClass="form-control">
                            <asp:ListItem Text="Select iPhone Model" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <br />
                    <div class="row">
                        <asp:DropDownList ID="ddliPad" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddliPad_SelectedIndexChanged" CssClass="form-control">
                            <asp:ListItem Text="Select iPad Model" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <br />
                    <div class="row">
                        <asp:DropDownList ID="ddliPod" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddliPod_SelectedIndexChanged" CssClass="form-control">
                            <asp:ListItem Text="Select iPod Model" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <br />
                    <div class="row">
                        <asp:DropDownList ID="ddlWatch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlWatch_SelectedIndexChanged" CssClass="form-control">
                            <asp:ListItem Text="Select Apple Watch Model" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <br />
                    <div class="row">
                        <asp:DropDownList ID="ddlAudioAccessory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAudioAccessory_SelectedIndexChanged" CssClass="form-control">
                            <asp:ListItem Text="Select HomePod Model" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <br />
                    <div class="row">
                        <asp:DropDownList ID="ddlAppleTV" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAppleTV_SelectedIndexChanged" CssClass="form-control">
                            <asp:ListItem Text="Select Apple TV Model" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <br />
                </div>

                <div class="col-sm" style="margin-top: 30px;">
                    <h4>
                        <asp:Label ID="lblStep3" runat="server" Text="Step 3"></asp:Label></h4>
                    <br />
                    <asp:Label ID="lblSelectionComment" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="lblSelection" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="btnRetrieve" runat="server" OnClick="btnRetrieve_Click" class="btn btn-primary" />
                </div>

            </div>

            <div class="row" style="margin-top: 30px;">
                <div class="col-sm">
                    <div class="card">
                        <div class="card-body">
                            <asp:Label ID="tbData" runat="server" Visible="false"></asp:Label>
                        </div>
                    </div>
                    
                </div>
            </div>

        </div>
        <br />
    </form>
</body>

</html>
