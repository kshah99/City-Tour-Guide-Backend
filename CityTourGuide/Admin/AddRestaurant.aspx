<%@ Page Title="Add Restaurant" Language="C#" MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true" CodeFile="AddRestaurant.aspx.cs" Inherits="Admin_AddRestaurant" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .cat label
        {
            display: inline-block;
            padding-left: 5px;
        }
    </style>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery('#lstRestaurant').addClass('active');
        });
    </script>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false&libraries=places"></script>
    <script type="text/javascript">
        var map;
        var myLatlng;
        var autocomplete;
        var new_place;
        var new_latitude = '';
        var new_longitude = '';

        function getPlaceData() {
            autocomplete = new google.maps.places.Autocomplete((document.getElementById('txtNewLocation')), { types: ['geocode'] });
            google.maps.event.addListener(autocomplete, 'place_changed', function () {
                var place = autocomplete.getPlace();

                new_place = place.name;
                new_latitude = place.geometry.location.lat();
                new_longitude = place.geometry.location.lng();
            });
        }

        function getNewMap() {
            handleNoGeolocation(false, new_latitude, new_longitude);
        }

        function initialize() {
            var myOptions = {
                zoom: 15,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    myLatlng = new google.maps.LatLng(position.coords.latitude,
                                       position.coords.longitude);

                    document.getElementById('<%= hdnLatitude.ClientID %>').value = position.coords.latitude;
                    document.getElementById('<%= hdnLongitude.ClientID %>').value = position.coords.longitude;

                    var marker = new google.maps.Marker({
                        draggable: true,
                        position: myLatlng,
                        map: map
                    });

                    google.maps.event.addListener(marker, 'dragend', function (event) {
                        document.getElementById('<%= hdnLatitude.ClientID %>').value = event.latLng.lat();
                        document.getElementById('<%= hdnLongitude.ClientID %>').value = event.latLng.lng();
                    });

                    map.setCenter(myLatlng);
                }, function () {
                    handleNoGeolocation(true, 0, 0);
                });
            } else {
                // Browser doesn't support Geolocation
                handleNoGeolocation(false, 0, 0);
            }
        }

        function handleNoGeolocation(errorFlag, Latitude, Longitude) {
            if (Latitude == 0 && Longitude == 0) {
                var myLatlng = new google.maps.LatLng(17.3660, 78.4760);

                document.getElementById('<%= hdnLatitude.ClientID %>').value = 17.3660;
                document.getElementById('<%= hdnLongitude.ClientID %>').value = 78.4760;
            } else {
                var myLatlng = new google.maps.LatLng(Latitude, Longitude);

                document.getElementById('<%= hdnLatitude.ClientID %>').value = Latitude;
                document.getElementById('<%= hdnLongitude.ClientID %>').value = Longitude;
            }

            var myOptions = {
                zoom: 15,
                center: myLatlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

            var marker = new google.maps.Marker({
                draggable: true,
                position: myLatlng,
                map: map
            });

            google.maps.event.addListener(marker, 'dragend', function (event) {
                document.getElementById('<%= hdnLatitude.ClientID %>').value = event.latLng.lat();
                document.getElementById('<%= hdnLongitude.ClientID %>').value = event.latLng.lng();
            });
        }

        window.onload = function () {
            initialize();
            getPlaceData();
        }
    </script>
    <script src="js/jquery.validate.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= btnAddRestaurant.ClientID %>').click(function (e) {
            
                $("#form1").validate({
                    rules: {
                        <%=txtRestaurantName.UniqueID %>: {
                            required: true
                        },
                        <%=txtRestaurantDescription.UniqueID %>: {
                            required: true
                        },
                        <%=txtRestaurantAddress.UniqueID %>: {
                            required: true
                        },
                         <%=txtPincode.UniqueID %>: {
                            required: true,
                            digits: true
                        },
                        <%=txtContactNumber.UniqueID %>: {
                            required: true,
                            digits: true
                        }
                    }, messages: {
                        <%=txtRestaurantName.UniqueID %>:{
                            required: "Please enter Restaurant Name."
                        },
                        <%=txtRestaurantDescription.UniqueID %>:{
                            required: "Please enter Restaurant Description."
                        },
                        <%=txtRestaurantAddress.UniqueID %>:{
                            required: "Please enter Restaurant Address."
                        },
                        <%=txtPincode.UniqueID %>:{
                            required: "Please enter Pincode.",
                            digits: "Please enter Valid Pincode."
                        },
                        <%=txtContactNumber.UniqueID %>:{
                            required: "Please enter Contact Number.",
                            digits: "Please enter Valid Contact Number."
                        }
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hdnLatitude" runat="server" Value="22.315564" />
    <asp:HiddenField ID="hdnLongitude" runat="server" Value="73.1762511" />
    <div class="span12">
        <div class="widget">
            <div class="widget-header">
                <i class="icon-suitcase"></i>
                <h3>
                    Add Restaurant
                </h3>
            </div>
            <div class="widget-content form-horizontal" id="formcontrols">
                <div class="control-group">
                    <label class="control-label" for="productname">
                        Restaurant Category</label>
                    <div class="controls">
                        <asp:CheckBoxList ID="chkCategory" runat="server" CssClass="cat">
                        </asp:CheckBoxList>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="productname">
                        Restaurant Name</label>
                    <div class="controls">
                        <asp:TextBox ID="txtRestaurantName" runat="server" CssClass="span4" placeholder="Enter Restaurant Name"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="productname">
                        Restaurant Description</label>
                    <div class="controls">
                        <asp:TextBox ID="txtRestaurantDescription" runat="server" CssClass="span4" placeholder="Enter Restaurant Description"
                            TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="productname">
                        Main Photo</label>
                    <div class="controls">
                        <asp:FileUpload ID="fuRestaurantMainPhoto" runat="server" CssClass="span4" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="productname">
                        Cover Photo</label>
                    <div class="controls">
                        <asp:FileUpload ID="fuRestaurantCoverPhoto" runat="server" CssClass="span4" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="productname">
                        Restaurant Address</label>
                    <div class="controls">
                        <asp:TextBox ID="txtRestaurantAddress" runat="server" CssClass="span4" placeholder="Enter Restaurant Address"
                            TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="productname">
                        Pincode</label>
                    <div class="controls">
                        <asp:TextBox ID="txtPincode" runat="server" CssClass="span4" placeholder="Enter Pincode"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="productname">
                        Contact Number</label>
                    <div class="controls">
                        <asp:TextBox ID="txtContactNumber" runat="server" CssClass="span4" placeholder="Enter Contact Number"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="productname">
                        Alternate Number</label>
                    <div class="controls">
                        <asp:TextBox ID="txtAlternateNumber" runat="server" CssClass="span4" placeholder="Enter Alternate Number"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="productname">
                        Email Address</label>
                    <div class="controls">
                        <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="span4" placeholder="Enter Email Address"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="productname">
                        Website</label>
                    <div class="controls">
                        <asp:TextBox ID="txtWebsite" runat="server" CssClass="span4" placeholder="Enter Restaurant Website"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        Publish
                    </label>
                 <div class="controls">
                        <asp:CheckBox ID="chkPublish" runat="server" Checked="true"></asp:CheckBox>
                    </div>
                </div>
                <div class="control-group" style="margin-top: 35px;">
                    <input type="text" id="txtNewLocation" name="txtNewLocation" placeholder="Find Restaurant Location"
                        title="Find Restaurant Location" autocomplete="off" />
                    <input type="button" id="btnGo" name="btnGo" class="btn btn-primary" value="Go" class="btn btn-small"
                        onclick="getNewMap();" />
                </div>
                <div class="control-group">
                    <div id="map_canvas" style="width: 100%; height: 250px;">
                    </div>
                </div>
                <div class="clearfix">
                </div>
                <div class="form-actions">
                    <asp:Button ID="btnAddRestaurant" runat="server" CssClass="btn btn-primary" Text="Add Restaurant"
                        OnClick="btnAddRestaurant_Click" />
                    <div class="clearfix">
                    </div>
                    <asp:Label ID="lblNote" runat="server" CssClass="SuccessMsg" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
