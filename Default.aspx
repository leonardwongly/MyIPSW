<%@ Page Title="" Language="C#" MasterPageFile="~/UI.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MyIPSW.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblName" runat="server"></asp:Label>
    <br/>
    <asp:DropDownList ID="ddliPhone" runat="server">
        <asp:ListItem Text="Select iPhone Model" Selected="True"></asp:ListItem>
    </asp:DropDownList>
</asp:Content>
