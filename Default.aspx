<%@ Page Title="" Language="C#" MasterPageFile="~/UI.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MyIPSW.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <div class="row">
            <div class="col-sm" style="margin-top: 30px">
                <h4>Step 1</h4>
                <br />
                <div class="row">
                    <asp:DropDownList ID="ddliPhone" runat="server" OnSelectedIndexChanged="ddliPhone_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Text="Select iPhone Model" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <br />
                <div class="row">
                    <asp:DropDownList ID="ddliPad" runat="server">
                        <asp:ListItem Text="Select iPad Model" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <br />
                <div class="row">
                    <asp:DropDownList ID="ddlWatch" runat="server">
                        <asp:ListItem Text="Select Watch Model" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <br />
                <div class="row">
                    <asp:DropDownList ID="ddlAudioAccessory" runat="server">
                        <asp:ListItem Text="Select HomePod Model" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <br />
                <div class="row">
                    <asp:DropDownList ID="ddliPod" runat="server">
                        <asp:ListItem Text="Select iPod Model" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <br />
                <div class="row">
                    <asp:DropDownList ID="ddlAppleTV" runat="server">
                        <asp:ListItem Text="Select Apple TV Model" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                </div>

            </div>

            <div class="col-sm" style="margin-top: 30px;">
                <h4><asp:Label ID="lblStep2" runat="server" Text="Step 2"></asp:Label></h4>
                <br />
                <asp:Label ID="lblSelectionComment" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblSelection" runat="server"></asp:Label>
                <br />
                <br />
                <asp:Button ID="btnRetrieve" runat="server" />
            </div>

        </div>
    </div>
    <br />

</asp:Content>
