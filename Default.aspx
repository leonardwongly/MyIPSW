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
                    <asp:DropDownList ID="ddliPhone" runat="server">
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
                <h4>Step 2</h4>
            </div>

        </div>
    </div>
    <br />

</asp:Content>
