<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Admin Login | City Tour Guide</title>
    <link href="Admin/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="Admin/css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
    <link href="Admin/css/font-awesome.css" rel="stylesheet" />
    <link href="http://fonts.googleapis.com/css?family=Open+Sans:400italic,600italic,400,600"
        rel="stylesheet" />
    <link href="Admin/css/style.css" rel="stylesheet" type="text/css" />
    <link href="Admin/css/pages/signin.css" rel="stylesheet" type="text/css" />
    <link href="favicon.ico" rel="icon" type="image/x-icon" />
    <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <script src="Admin/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="Admin/js/jquery.validate.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= btnLogin.ClientID %>').click(function (e) {
                
                $("#form1").validate({
                    rules: {
                        <%=txtEmail.UniqueID %>: {
                            required: true,
                            email: true
                        },
                        <%=txtPassword.UniqueID %>: {				
                            required: true
                        }
                    }, messages: {
                        <%=txtEmail.UniqueID %>:{
                            required: "Please enter Email Address.",
                            email: "Please enter Valid Email Address."
                        },
                        <%=txtPassword.UniqueID %>:{
                            required: "Please enter Password."
                        }
                    }
                });

            if($("#form1").valid()) {
                e.preventDefault();

                $('#<%= lblNote.ClientID %>').hide();
                $('#<%= btnLogin.ClientID %>').val('Please Wait...');
                $('#<%= btnLogin.ClientID %>').attr('disabled', true);

                $.ajax({
                    type: "POST",
                    url: "Login.aspx/AdminLogin",
                    data: '{ "Email" :"' + $('#<%= txtEmail.ClientID %>').val() + '", "Password" :"' + $('#<%= txtPassword.ClientID %>').val() + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                    if(msg.d == 'success'){
                        
                        window.location = "Admin/Dashboard.aspx"
                        
                            $('#<%= lblNote.ClientID %>').html('Login is Successfull.');
                            $('#<%= lblNote.ClientID %>').removeClass('ErrorMsg').addClass('SuccessMsg');
                        
                            $('#<%= btnLogin.ClientID %>').attr('disabled', true);
                        }else if(msg.d == 'invalid'){
                            $('#<%= lblNote.ClientID %>').html('Email Address or Password is Wrong.');
                            $('#<%= lblNote.ClientID %>').removeClass('SuccessMsg').addClass('ErrorMsg');

                            $('#<%= btnLogin.ClientID %>').attr('disabled', false);
                        }else if(msg.d == 'not_exist'){
                            $('#<%= lblNote.ClientID %>').html('Your Email Address is not Registered.');
                            $('#<%= lblNote.ClientID %>').removeClass('SuccessMsg').addClass('ErrorMsg');

                            $('#<%= btnLogin.ClientID %>').attr('disabled', false);
                        }else if(msg.d == 'unauthorize'){
                            $('#<%= lblNote.ClientID %>').html('You are not authorized to Login. Please Contact Administrator.');
                            $('#<%= lblNote.ClientID %>').removeClass('SuccessMsg').addClass('ErrorMsg');

                            $('#<%= btnLogin.ClientID %>').attr('disabled', false);
                        }else {
                            $('#<%= lblNote.ClientID %>').html('Something is Wrong. Please try after sometime!');
                            $('#<%= lblNote.ClientID %>').removeClass('SuccessMsg').addClass('ErrorMsg');

                            $('#<%= btnLogin.ClientID %>').attr('disabled', false);
                        }
                            $('#<%= lblNote.ClientID %>').show();
                            $('#<%= lblNote.ClientID %>').attr('style', 'display:inline-block');
                            $('#<%= lblNote.ClientID %>').focus();

                            $('#<%= btnLogin.ClientID %>').val('Sign In');
                    },
                    error: function (msg) {
                        $('#<%= lblNote.ClientID %>').html('Something is Wrong. Please try after sometime!');
                        $('#<%= lblNote.ClientID %>').removeClass('SuccessMsg').addClass('ErrorMsg');

                        $('#<%= lblNote.ClientID %>').show();
                        $('#<%= lblNote.ClientID %>').attr('style', 'display:inline-block');

                        $('#<%= btnLogin.ClientID %>').val('Sign In');
                        $('#<%= btnLogin.ClientID %>').attr('disabled', false);
                    }
                });
                }else{
                    $('#<%= lblNote.ClientID %>').hide();
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="navbar navbar-fixed-top">
        <div class="navbar-inner">
            <div class="container">
                <a class="brand" href="Home">City Tour Guide </a>
            </div>
        </div>
    </div>
    <div class="account-container">
        <div class="content clearfix">
            <h1>
                Admin Login</h1>
            <div class="login-fields">
                <p>
                    Please provide your details</p>
                <div class="field">
                    <asp:TextBox ID="txtEmail" runat="server" placeholder="Email Address" CssClass="login username-field"></asp:TextBox>
                </div>
                <div class="field">
                    <asp:TextBox ID="txtPassword" runat="server" placeholder="Password" CssClass="login password-field"
                        TextMode="Password"></asp:TextBox>
                </div>
            </div>
            <div class="login-actions">
                <asp:Button ID="btnLogin" runat="server" CssClass="button btn btn-success btn-large"
                    Text="Sign In" />
                <div class="clearfix">
                </div>
                <asp:Label ID="lblNote" runat="server" CssClass="SuccessMsg" Style="display: none;"></asp:Label>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
