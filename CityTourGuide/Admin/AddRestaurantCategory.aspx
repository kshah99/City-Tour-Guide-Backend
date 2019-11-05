<%@ Page Title="Add Restaurant Category" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" 
CodeFile="AddRestaurantCategory.aspx.cs" Inherits="Admin_AddRestaurantCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
    jQuery(document).ready(function () {
        jQuery('#lstCategory').addClass('active');
    });
    </script>
    <script src="js/jquery.validate.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= btnAddCategory.ClientID %>').click(function (e) {
            
                $("#form1").validate({
                    rules: {
                        <%=txtCategoryName.UniqueID %>: {
                            required: true
                        }
                    }, messages: {
                        <%=txtCategoryName.UniqueID %>:{
                            required: "Please enter Category Name."
                        }
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="span12">
        <div class="widget">
            <div class="widget-header">
                <i class="icon-suitcase"></i>
                <h3>
                    Add Restaurant Category</h3>
            </div>
            <div class="widget-content form-horizontal" id="formcontrols">
                <div class="control-group">
                    <label class="control-label" for="productname">
                        Category Name</label>
                    <div class="controls">
                        <asp:TextBox ID="txtCategoryName" runat="server" CssClass="span4" placeholder="Enter Category Name"></asp:TextBox>
                    </div>
                </div>
                <div class="clearfix">
                </div>
                <div class="form-actions">
                    <asp:Button ID="btnAddCategory" runat="server" CssClass="btn btn-primary" Text="Add Category"
                        OnClick="btnAddCategory_Click" />
                    <div class="clearfix">
                    </div>
                    <asp:Label ID="lblNote" runat="server" CssClass="SuccessMsg" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

