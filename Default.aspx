<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ipsw.Default" Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="Find iOS firmware files quickly with a guided workflow." />
    <meta name="author" content="Leonard Wong, IPSW" />
    <title>IPSW - Unified Firmware Finder</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css"
        integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN"
        crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" />
    <script defer src='https://static.cloudflareinsights.com/beacon.min.js'
        data-cf-beacon='{"token": "e955a12c2d2043568d3806c104d546e1"}'></script>

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
    <link rel="manifest" href="favicon/manifest.json" />
    <meta name="msapplication-TileColor" content="#ffffff" />
    <meta name="msapplication-TileImage" content="/ms-icon-144x144.png" />
    <meta name="theme-color" content="#ffffff" />

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
            font-weight: 700;
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
            background: #ffffff;
        }

        .step-card:hover {
            transform: translateY(-3px);
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
            font-weight: 700;
        }

        .option-list input {
            margin-right: 0.5rem;
        }

        .option-list label {
            display: flex;
            align-items: center;
            gap: 0.5rem;
            padding: 0.45rem 0.75rem;
            border: 1px solid #dbe3ee;
            border-radius: 0.75rem;
            margin-bottom: 0.5rem;
            background: #fff;
            transition: all 0.2s ease;
            cursor: pointer;
        }

        .option-list input:checked+label {
            background: #0d6efd;
            color: #fff;
            border-color: #0d6efd;
        }

        .form-select.step-select {
            border-radius: 0.75rem;
            padding: 0.7rem 1rem;
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
            background: rgba(255, 255, 255, 0.25);
        }

        .progress-bar {
            background: linear-gradient(90deg, #20c997, #0d6efd);
        }

        .btn-primary,
        .btn-outline-primary {
            border-radius: 999px;
            font-weight: 600;
            padding: 0.7rem 1.4rem;
        }

        .results-card {
            border-radius: 1rem;
            border: 1px solid rgba(15, 23, 42, 0.08);
        }

        .results-table {
            border-radius: 1rem;
            overflow: hidden;
        }

        .small-helper {
            font-size: 0.9rem;
            color: #5b6778;
        }

        .sr-only {
            position: absolute;
            width: 1px;
            height: 1px;
            padding: 0;
            margin: -1px;
            overflow: hidden;
            clip: rect(0, 0, 0, 0);
            border: 0;
        }

        @media (max-width: 991px) {
            .page-hero {
                padding: 1.5rem;
            }
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
                        <a class="nav-link active" aria-current="page" href="Default.aspx">Find Firmware</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="Link.aspx">Legacy Link Entry</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#help">Help</a>
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
                        <span class="hero-badge mb-3"><i class="bi bi-lightning-charge"></i> Unified guided flow</span>
                        <h1 class="h2 fw-semibold mb-2">Find firmware files in three predictable steps.</h1>
                        <p class="mb-0">Choose a source, select a target, then decide whether you want a structured table or quick links.</p>
                    </div>
                    <div class="text-lg-end">
                        <p class="mb-1">Progress</p>
                        <div class="progress modern-progress" style="width: 220px;">
                            <div class="progress-bar" id="selectionProgress" role="progressbar" aria-valuemin="0"
                                aria-valuemax="100" aria-valuenow="0" style="width: 0%">0%</div>
                        </div>
                    </div>
                </div>
            </section>

            <asp:Panel ID="pnlStatus" runat="server" Visible="false"
                CssClass="alert alert-info d-flex justify-content-between align-items-center" role="alert">
                <span>
                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                </span>
                <asp:Button ID="btnRetry" runat="server" Text="Retry" CssClass="btn btn-sm btn-outline-dark"
                    Visible="false" OnClick="btnRetry_Click" />
            </asp:Panel>

            <div class="row g-4">
                <div class="col-lg-4">
                    <div class="card step-card h-100">
                        <div class="card-body">
                            <div class="d-flex align-items-center gap-3 mb-3">
                                <span class="step-number">1</span>
                                <div>
                                    <h4 class="mb-0">Select Source</h4>
                                    <small class="text-muted">Choose how to discover firmware files</small>
                                </div>
                            </div>
                            <asp:RadioButtonList ID="rblOptions" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="rblOptions_SelectedIndexChanged" CssClass="option-list"
                                RepeatLayout="Flow" RepeatDirection="Vertical">
                                <asp:ListItem Text="Official IPSW" Value="official"></asp:ListItem>
                                <asp:ListItem Text="OTA" Value="ota"></asp:ListItem>
                                <asp:ListItem Text="Version" Value="version"></asp:ListItem>
                                <asp:ListItem Text="Version (OTA)" Value="version_ota"></asp:ListItem>
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
                                    <small class="text-muted">Pick a device family or firmware version</small>
                                </div>
                            </div>
                            <div class="d-grid gap-3">
                                <asp:DropDownList ID="ddlVersion" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlVersion_SelectedIndexChanged" CssClass="form-select step-select">
                                    <asp:ListItem Text="Select iOS Version" Selected="True" Value=""></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddlVersionOTA" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlVersionOTA_SelectedIndexChanged" CssClass="form-select step-select">
                                    <asp:ListItem Text="Select OTA Version" Selected="True" Value=""></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddliPhone" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddliPhone_SelectedIndexChanged" CssClass="form-select step-select">
                                    <asp:ListItem Text="Select iPhone Model" Selected="True" Value=""></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddliPad" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddliPad_SelectedIndexChanged" CssClass="form-select step-select">
                                    <asp:ListItem Text="Select iPad Model" Selected="True" Value=""></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddliPod" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddliPod_SelectedIndexChanged" CssClass="form-select step-select">
                                    <asp:ListItem Text="Select iPod Model" Selected="True" Value=""></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddlMac" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlMac_SelectedIndexChanged" CssClass="form-select step-select">
                                    <asp:ListItem Text="Select Mac Model" Selected="True" Value=""></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddlWatch" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlWatch_SelectedIndexChanged" CssClass="form-select step-select">
                                    <asp:ListItem Text="Select Apple Watch Model" Selected="True" Value=""></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddlAudioAccessory" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlAudioAccessory_SelectedIndexChanged"
                                    CssClass="form-select step-select">
                                    <asp:ListItem Text="Select HomePod Model" Selected="True" Value=""></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddlAppleTV" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlAppleTV_SelectedIndexChanged" CssClass="form-select step-select">
                                    <asp:ListItem Text="Select Apple TV Model" Selected="True" Value=""></asp:ListItem>
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
                                    <small class="text-muted">Choose output mode and retrieve results</small>
                                </div>
                            </div>

                            <asp:RadioButtonList ID="rblResultMode" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="rblResultMode_SelectedIndexChanged" CssClass="option-list mb-3"
                                RepeatLayout="Flow" RepeatDirection="Vertical">
                                <asp:ListItem Text="Table View" Value="table" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Links View" Value="links"></asp:ListItem>
                            </asp:RadioButtonList>

                            <div class="selection-preview mb-4">
                                <asp:Label ID="lblSelectionComment" runat="server"></asp:Label>
                                <div class="mt-2 fw-semibold">
                                    <asp:Label ID="lblSelection" runat="server"></asp:Label>
                                </div>
                            </div>

                            <asp:Button ID="btnRetrieve" runat="server" Text="Retrieve" OnClick="btnRetrieve_Click"
                                CssClass="btn btn-primary btn-lg w-100 mt-auto" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-4">
                <div class="col-12">
                    <div class="card results-card">
                        <div class="card-body">
                            <div class="d-flex flex-column flex-md-row align-items-md-center justify-content-between gap-3 mb-3">
                                <div>
                                    <h5 class="mb-1">Results</h5>
                                    <p class="mb-0 small-helper">
                                        <asp:Label ID="lblResultSummary" runat="server"></asp:Label>
                                    </p>
                                </div>
                                <asp:Button ID="btnDownloadAll" runat="server" Text="Open All Links"
                                    CssClass="btn btn-outline-primary" Visible="false"
                                    OnClientClick="return confirmDownloadAll();" />
                            </div>

                            <asp:Table ID="tblData" runat="server" EnableViewState="false" BorderColor="#dbe3ee"
                                BorderStyle="Solid" BorderWidth="1px" CellPadding="8" CellSpacing="0" GridLines="Both"
                                CssClass="table table-hover align-middle results-table" Visible="false"></asp:Table>

                            <asp:Label ID="lblLinkResults" runat="server" EnableViewState="false" Visible="false"
                                Style="display:block; text-align:left;"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>

            <section id="help" class="mt-4">
                <div class="card step-card">
                    <div class="card-body">
                        <h5 class="mb-2">Need help?</h5>
                        <p class="mb-0 small-helper">If retrieval fails, retry once. If the issue persists, switch source mode or choose a different version, then retry.</p>
                    </div>
                </div>
            </section>

            <asp:HiddenField ID="hfDownloadLinks" runat="server" />
            <asp:HiddenField ID="hfTelemetryStatus" runat="server" />
            <asp:HiddenField ID="hfResultCount" runat="server" />
        </main>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"
        integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL"
        crossorigin="anonymous"></script>

    <script>
        (function () {
            const progressBar = document.getElementById('selectionProgress');
            const ddlVersionId = '<%= ddlVersion.ClientID %>';
            const ddlVersionOtaId = '<%= ddlVersionOTA.ClientID %>';
            const deviceSelectIds = [
                '<%= ddliPhone.ClientID %>',
                '<%= ddliPad.ClientID %>',
                '<%= ddliPod.ClientID %>',
                '<%= ddlWatch.ClientID %>',
                '<%= ddlAppleTV.ClientID %>',
                '<%= ddlMac.ClientID %>',
                '<%= ddlAudioAccessory.ClientID %>'
            ];

            const downloadLinksField = document.getElementById('<%= hfDownloadLinks.ClientID %>');
            const telemetryStatusField = document.getElementById('<%= hfTelemetryStatus.ClientID %>');
            const resultCountField = document.getElementById('<%= hfResultCount.ClientID %>');
            const retrieveButton = document.getElementById('<%= btnRetrieve.ClientID %>');

            function randomId(prefix) {
                const seed = Math.random().toString(36).slice(2) + Date.now().toString(36);
                return prefix + '-' + seed.slice(0, 24);
            }

            function getCookie(name) {
                const parts = document.cookie.split(';').map(v => v.trim());
                for (let i = 0; i < parts.length; i += 1) {
                    if (parts[i].startsWith(name + '=')) {
                        return decodeURIComponent(parts[i].slice(name.length + 1));
                    }
                }

                return '';
            }

            function setCookie(name, value, days) {
                const maxAge = days * 24 * 60 * 60;
                document.cookie = name + '=' + encodeURIComponent(value) + '; path=/; max-age=' + maxAge + '; SameSite=Lax';
            }

            function ensureSessionId() {
                let sessionId = getCookie('ipsw_session_id');
                if (!sessionId) {
                    sessionId = randomId('sess');
                    setCookie('ipsw_session_id', sessionId, 30);
                }

                return sessionId;
            }

            const sessionId = ensureSessionId();
            const journeyId = randomId('journey');

            function getSelectedRadioValue(nameFragment) {
                const selected = document.querySelector('input[type="radio"][name*="' + nameFragment + '"]:checked');
                return selected ? selected.value : '';
            }

            function isVisible(element) {
                return !!element && element.offsetParent !== null;
            }

            function getSelectionState() {
                const source = getSelectedRadioValue('rblOptions');
                const resultMode = getSelectedRadioValue('rblResultMode');
                const versionSelect = document.getElementById(ddlVersionId);
                const versionOtaSelect = document.getElementById(ddlVersionOtaId);

                let targetSelected = false;
                if (source === 'version' && versionSelect) {
                    targetSelected = versionSelect.selectedIndex > 0;
                } else if (source === 'version_ota' && versionOtaSelect) {
                    targetSelected = versionOtaSelect.selectedIndex > 0;
                } else {
                    for (let i = 0; i < deviceSelectIds.length; i += 1) {
                        const select = document.getElementById(deviceSelectIds[i]);
                        if (isVisible(select) && select.selectedIndex > 0) {
                            targetSelected = true;
                            break;
                        }
                    }
                }

                return {
                    source,
                    resultMode,
                    targetSelected
                };
            }

            function updateProgress() {
                const state = getSelectionState();
                let filled = 0;
                const total = 3;

                if (state.source) {
                    filled += 1;
                }

                if (state.targetSelected) {
                    filled += 1;
                }

                if (state.resultMode) {
                    filled += 1;
                }

                const percentage = Math.round((filled / total) * 100);
                if (progressBar) {
                    progressBar.style.width = percentage + '%';
                    progressBar.textContent = percentage + '%';
                    progressBar.setAttribute('aria-valuenow', String(percentage));
                }
            }

            function sendTelemetry(eventName, payload) {
                const state = getSelectionState();
                const body = Object.assign({
                    eventName: eventName,
                    sessionId: sessionId,
                    journeyId: journeyId,
                    source: state.source || '',
                    resultMode: state.resultMode || ''
                }, payload || {});

                fetch('Telemetry.ashx', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(body),
                    credentials: 'same-origin'
                }).catch(function () {
                    return null;
                });
            }

            window.confirmDownloadAll = function () {
                const raw = downloadLinksField ? downloadLinksField.value : '';
                const urls = raw.split('\n').map(v => v.trim()).filter(Boolean);
                sendTelemetry('DownloadAllClicked', { step: 'results' });

                if (urls.length === 0) {
                    sendTelemetry('ErrorShown', { status: 'error', errorCode: 'no_download_links' });
                    alert('No links are available yet. Retrieve results first.');
                    return false;
                }

                if (!window.confirm('Open ' + urls.length + ' links in new tabs?')) {
                    return false;
                }

                sendTelemetry('DownloadAllConfirmed', { step: 'results', status: 'confirmed' });

                let blocked = 0;
                for (let i = 0; i < urls.length; i += 1) {
                    const opened = window.open(urls[i], '_blank', 'noopener');
                    if (!opened) {
                        blocked += 1;
                    }
                }

                if (blocked > 0) {
                    alert('Your browser blocked ' + blocked + ' popup(s). Allow popups for this site and retry.');
                    sendTelemetry('DownloadAllBlocked', { status: 'partial', errorCode: 'popup_blocked' });
                }

                return false;
            };

            document.addEventListener('change', function (event) {
                if (event.target.matches('select.step-select') || event.target.matches('input[type="radio"][name*="rblOptions"]') || event.target.matches('input[type="radio"][name*="rblResultMode"]')) {
                    updateProgress();
                }

                if (event.target.matches('input[type="radio"][name*="rblOptions"]')) {
                    sendTelemetry('SourceSelected', { step: 'source', status: 'changed' });
                }

                if (event.target.matches('input[type="radio"][name*="rblResultMode"]')) {
                    sendTelemetry('StepCompleted', { step: 'result_mode', status: 'selected' });
                }

                if (event.target.matches('select.step-select')) {
                    const state = getSelectionState();
                    if (state.targetSelected) {
                        sendTelemetry('StepCompleted', { step: 'target', status: 'selected' });
                    }
                }
            });

            if (retrieveButton) {
                retrieveButton.addEventListener('click', function () {
                    sendTelemetry('RetrieveClicked', { step: 'retrieve', status: 'started' });
                });
            }

            document.addEventListener('DOMContentLoaded', function () {
                updateProgress();
                sendTelemetry('PageViewed', { step: 'entry', status: 'loaded' });

                const telemetryStatus = telemetryStatusField ? telemetryStatusField.value : '';
                const resultCount = resultCountField ? Number(resultCountField.value || '0') : 0;

                if (telemetryStatus === 'success') {
                    sendTelemetry('ResultsRendered', { step: 'results', status: 'success', durationMs: 0, errorCode: '' });
                } else if (telemetryStatus === 'empty') {
                    sendTelemetry('ResultsRendered', { step: 'results', status: 'empty', durationMs: 0, errorCode: '' });
                } else if (telemetryStatus === 'error') {
                    sendTelemetry('ErrorShown', { step: 'results', status: 'error', errorCode: 'retrieve_failed' });
                }

                if (resultCount > 0 && telemetryStatus === 'success') {
                    sendTelemetry('StepCompleted', { step: 'task_complete', status: 'success', durationMs: 0, errorCode: '' });
                }
            });
        }());
    </script>
</body>

</html>
