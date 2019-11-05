<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true"
    CodeFile="Dashboard.aspx.cs" Inherits="Admin_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <script type="text/javascript">
     jQuery(document).ready(function () {
         jQuery('#lstDashboard').addClass('active');
     });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="span12">
        <div class="widget">
            <div class="widget-header">
                <i class="icon-suitcase"></i>
                <h3>
                    City Tour Guide</h3>
            </div>
            <div class="widget-content form-horizontal" id="formcontrols">
                Welcome to City Tour Guide,
            </div>
        </div>
    </div>
</asp:Content>
