<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ipsw.Default" Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="" />
    <meta name="author" content="Leonard Wong, IPSW" />
    <title>IPSW</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css"
        integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN"
        crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" />
    <!-- Cloudflare Web Analytics -->
    <script defer src='https://static.cloudflareinsights.com/beacon.min.js'
        data-cf-beacon='{"token": "e955a12c2d2043568d3806c104d546e1"}'></script>
    <!-- End Cloudflare Web Analytics -->
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
    <link rel="manifest" href="manifest.json" />
    <meta name="msapplication-TileColor" content="#ffffff">
    <meta name="msapplication-TileImage" content="/ms-icon-144x144.png">
    <meta name="theme-color" content="#ffffff">

    <script type="module">
        import 'https://cdn.jsdelivr.net/npm/@pwabuilder/pwaupdate';
        const el = document.createElement('pwa-update');
        document.body.appendChild(el);
    </script>

    <style>
        :root {
            color-scheme: light;
        }

        body.page-body {
            background: radial-gradient(circle at top, #f8fbff, #edf2f7 55%, #e7ecf6);
            font-family: "Inter", "Segoe UI", system-ui, -apple-system, sans-serif;
            min-height: 100vh;
        }

        .navbar-brand {
            font-weight: 600;
            letter-spacing: 0.04em;
        }

        .page-hero {
            background: linear-gradient(135deg, #0d6efd, #5c7cfa);
            color: #fff;
            border-radius: 1.25rem;
            padding: 2rem;
            box-shadow: 0 20px 45px rgba(13, 110, 253, 0.25);
        }

        .page-hero .hero-badge {
            background: rgba(255, 255, 255, 0.2);
            border: 1px solid rgba(255, 255, 255, 0.3);
            border-radius: 999px;
            padding: 0.35rem 0.8rem;
            display: inline-flex;
            gap: 0.5rem;
            align-items: center;
            font-size: 0.85rem;
        }

        .step-card {
            border-radius: 1rem;
            border: 1px solid rgba(15, 23, 42, 0.08);
            box-shadow: 0 14px 35px rgba(15, 23, 42, 0.08);
            transition: transform 0.2s ease, box-shadow 0.2s ease;
        }

        .step-card:hover {
            transform: translateY(-4px);
            box-shadow: 0 18px 45px rgba(15, 23, 42, 0.12);
        }

        .step-number {
            width: 40px;
            height: 40px;
            border-radius: 50%;
            background: rgba(13, 110, 253, 0.12);
            color: #0d6efd;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            font-weight: 600;
        }

        .option-list input {
            margin-right: 0.5rem;
        }

        .option-list label {
            display: flex;
            align-items: center;
            gap: 0.5rem;
            padding: 0.35rem 0.75rem;
            border: 1px solid #e2e8f0;
            border-radius: 999px;
            margin-bottom: 0.5rem;
            background: #fff;
            transition: all 0.2s ease;
            cursor: pointer;
        }

        .option-list input:checked + label {
            background: #0d6efd;
            color: #fff;
            border-color: #0d6efd;
        }

        .form-select.step-select {
            border-radius: 0.75rem;
            padding: 0.75rem 1rem;
            box-shadow: none;
            transition: box-shadow 0.2s ease, border-color 0.2s ease;
        }

        .form-select.step-select:focus {
            border-color: #5c7cfa;
            box-shadow: 0 0 0 0.25rem rgba(92, 124, 250, 0.2);
        }

        .selection-preview {
            background: #f8fafc;
            border-radius: 1rem;
            padding: 1.25rem;
            border: 1px dashed #cbd5f5;
        }

        .progress.modern-progress {
            height: 0.65rem;
            border-radius: 999px;
            overflow: hidden;
        }

        .progress-bar {
            background: linear-gradient(90deg, #0d6efd, #5c7cfa);
        }

        .results-table {
            border-radius: 1rem;
            overflow: hidden;
        }

        .results-table table {
            margin-bottom: 0;
        }

        .btn-primary {
            border-radius: 999px;
            padding: 0.75rem 1.5rem;
            font-weight: 600;
            box-shadow: 0 14px 25px rgba(13, 110, 253, 0.25);
        }
    </style>
</head>

<body class="page-body">
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark shadow-sm">
        <div class="container">
            <a class="navbar-brand" href="Default.aspx">IPSW</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav"
                aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav ms-auto">
                    <li class="nav-item">
                        <a class="nav-link active" aria-current="page" href="Default.aspx">Home</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="Link.aspx">Link</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

    <form id="form1" runat="server">
        <main class="container py-4">
            <section class="page-hero mb-4">
                <div class="d-flex flex-column flex-lg-row align-items-start align-items-lg-center justify-content-between gap-3">
                    <div>
                        <span class="hero-badge mb-3"><i class="bi bi-lightning-charge"></i> Fast IPSW discovery</span>
                        <h1 class="h2 fw-semibold mb-2">Find the right IPSW in three guided steps.</h1>
                        <p class="mb-0">Choose your source, pick a version and device, then retrieve the latest download details.</p>
                    </div>
                    <div class="text-lg-end">
                        <p class="mb-1">Progress</p>
                        <div class="progress modern-progress" style="width: 220px;">
                            <div class="progress-bar" id="selectionProgress" role="progressbar" style="width: 0%">0%</div>
                        </div>
                    </div>
                </div>
            </section>

            <div class="row g-4">
                <div class="col-lg-4">
                    <div class="card step-card h-100">
                        <div class="card-body">
                            <div class="d-flex align-items-center gap-3 mb-3">
                                <span class="step-number">1</span>
                                <div>
                                    <h4 class="mb-0">Select Source</h4>
                                    <small class="text-muted">Official, OTA, or by version</small>
                                </div>
                            </div>
                            <asp:RadioButtonList ID="rblOptions" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="rblOptions_SelectedIndexChanged" CssClass="option-list"
                                RepeatLayout="Flow" RepeatDirection="Vertical">
                                <asp:ListItem>Official</asp:ListItem>
                                <asp:ListItem>OTA</asp:ListItem>
                                <asp:ListItem>Version</asp:ListItem>
                                <asp:ListItem>Version (OTA)</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>

                <div class="col-lg-4">
                    <div class="card step-card h-100">
                        <div class="card-body">
                            <div class="d-flex align-items-center gap-3 mb-3">
                                <span class="step-number">2</span>
                                <div>
                                    <h4 class="mb-0">
                                        <asp:Label ID="lblStep2" runat="server" Text="Step 2"></asp:Label>
                                    </h4>
                                    <small class="text-muted">Pick a version and device family</small>
                                </div>
                            </div>
                            <div class="d-grid gap-3">
                                <asp:DropDownList ID="ddlVersion" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlVersion_SelectedIndexChanged" CssClass="form-select step-select">
                                    <asp:ListItem Text="Select Version" Selected="True"></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddlVersionOTA" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlVersionOTA_SelectedIndexChanged"
                                    CssClass="form-select step-select">
                                    <asp:ListItem Text="Select Version (OTA)" Selected="True"></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddliPhone" runat="server"
                                    OnSelectedIndexChanged="ddliPhone_SelectedIndexChanged" AutoPostBack="True"
                                    CssClass="form-select step-select">
                                    <asp:ListItem Text="Select iPhone Model" Selected="True"></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddliPad" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddliPad_SelectedIndexChanged" CssClass="form-select step-select">
                                    <asp:ListItem Text="Select iPad Model" Selected="True"></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddliPod" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddliPod_SelectedIndexChanged" CssClass="form-select step-select">
                                    <asp:ListItem Text="Select iPod Model" Selected="True"></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddlMac" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlMac_SelectedIndexChanged" CssClass="form-select step-select">
                                    <asp:ListItem Text="Select Mac Model" Selected="True"></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddlWatch" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlWatch_SelectedIndexChanged" CssClass="form-select step-select">
                                    <asp:ListItem Text="Select Apple Watch Model" Selected="True"></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddlAudioAccessory" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlAudioAccessory_SelectedIndexChanged" CssClass="form-select step-select">
                                    <asp:ListItem Text="Select HomePod Model" Selected="True"></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddlAppleTV" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlAppleTV_SelectedIndexChanged" CssClass="form-select step-select">
                                    <asp:ListItem Text="Select Apple TV Model" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-4">
                    <div class="card step-card h-100">
                        <div class="card-body d-flex flex-column">
                            <div class="d-flex align-items-center gap-3 mb-3">
                                <span class="step-number">3</span>
                                <div>
                                    <h4 class="mb-0">
                                        <asp:Label ID="lblStep3" runat="server" Text="Step 3"></asp:Label>
                                    </h4>
                                    <small class="text-muted">Review and retrieve your IPSW</small>
                                </div>
                            </div>
                            <div class="selection-preview mb-4">
                                <asp:Label ID="lblSelectionComment" runat="server"></asp:Label>
                                <div class="mt-2">
                                    <asp:Label ID="lblSelection" runat="server"></asp:Label>
                                </div>
                            </div>
                            <asp:Button ID="btnRetrieve" runat="server" OnClick="btnRetrieve_Click"
                                CssClass="btn btn-primary btn-lg w-100 mt-auto" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-4">
                <div class="col-12">
                    <div class="card step-card results-table">
                        <div class="card-body p-0">
                            <asp:Table ID="tblData" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                CellPadding="3" CellSpacing="3" GridLines="Both" CssClass="table table-hover align-middle">
                            </asp:Table>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"
        integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL"
        crossorigin="anonymous"></script>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            const progressBar = document.getElementById('selectionProgress');
            const selects = Array.from(document.querySelectorAll('select.step-select'));

            function updateProgress() {
                let filled = 0;
                const total = selects.length + 1;
                const radioChecked = document.querySelector('input[type="radio"][name*="rblOptions"]:checked');

                if (radioChecked) {
                    filled += 1;
                }

                selects.forEach(function (select) {
                    if (select.selectedIndex > 0) {
                        filled += 1;
                    }
                });

                const percentage = Math.round((filled / total) * 100);
                if (progressBar) {
                    progressBar.style.width = percentage + '%';
                    progressBar.textContent = percentage + '%';
                }
            }

            document.addEventListener('change', function (event) {
                if (event.target.matches('select.step-select') || event.target.matches('input[type="radio"][name*="rblOptions"]')) {
                    updateProgress();
                }
            });

            updateProgress();
        });
    </script>
</body>

</html>
