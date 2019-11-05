﻿<%@ Page Title="Add Attraction" Language="C#" MasterPageFile="~/Admin/AdminMaster.master"
    AutoEventWireup="true" CodeFile="AddAttraction.aspx.cs" Inherits="Admin_AddAttraction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery('#lstAttraction').addClass('active');
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
            $('#<%= btnAddAttraction.ClientID %>').click(function (e) {
            
                $("#form1").validate({
                    rules: {
                        <%=txtAttractionName.UniqueID %>: {
                            required: true
                        },
                        <%=txtAttractionDescription.UniqueID %>: {
                            required: true
                        },
                        <%=txtAttractionAddress.UniqueID %>: {
                            required: true
                        },
                        <%=txtPincode.UniqueID %>: {
                            required: true,
                            digits: true
                        },
                    }, messages: {
                        <%=txtAttractionName.UniqueID %>:{
                            required: "Please enter Attraction Name."
                        },
                        <%=txtAttractionDescription.UniqueID %>:{
                            required: "Please enter Attraction Description."
                        },
                        <%=txtAttractionAddress.UniqueID %>:{
                            required: "Please enter Attraction Address."
                        },
                        <%=txtPincode.UniqueID %>:{
                            required: "Please enter Pincode.",
                            digits: "Please enter Valid Pincode."
                        },
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
                    Add Attraction
                </h3>
            </div>
            <div class="widget-content form-horizontal" id="formcontrols">
                <div class="control-group">
                    <label class="control-label">
                        Attraction Name</label>
                    <div class="controls">
                        <asp:TextBox ID="txtAttractionName" runat="server" CssClass="span4" placeholder="Enter Attraction Name"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        Attraction Description</label>
                    <div class="controls">
                        <asp:TextBox ID="txtAttractionDescription" runat="server" CssClass="span4" placeholder="Enter Attraction Description"
                            TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        Main Photo</label>
                    <div class="controls">
                        <asp:FileUpload ID="fuAttractionMainPhoto" runat="server" CssClass="span4" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        Cover Photo</label>
                    <div class="controls">
                        <asp:FileUpload ID="fuAttractionCoverPhoto" runat="server" CssClass="span4" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        Attraction Address</label>
                    <div class="controls">
                        <asp:TextBox ID="txtAttractionAddress" runat="server" CssClass="span4" placeholder="Enter Attraction Address"
                            TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        Pincode
                    </label>
                    <div class="controls">
                        <asp:TextBox ID="txtPincode" runat="server" CssClass="span4" placeholder="Enter Pincode"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        Contact Number</label>
                    <div class="controls">
                        <asp:TextBox ID="txtContactNumber" runat="server" CssClass="span4" placeholder="Enter Contact Number"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        Alternate Number</label>
                    <div class="controls">
                        <asp:TextBox ID="txtAlternateNumber" runat="server" CssClass="span4" placeholder="Enter Alternate Number"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        Email Address</label>
                    <div class="controls">
                        <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="span4" placeholder="Enter Email Address"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        Website</label>
                    <div class="controls">
                        <asp:TextBox ID="txtWebsite" runat="server" CssClass="span4" placeholder="Enter Website"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label">
                        In Vadodara
                    </label>
                    <div class="controls">
                        <asp:CheckBox ID="chkInVadodara" runat="server" Checked="true"></asp:CheckBox>
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
                    <input type="text" id="txtNewLocation" name="txtNewLocation" placeholder="Find Attraction Location"
                        title="Find Attraction Location" autocomplete="off" />
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
                    <asp:Button ID="btnAddAttraction" runat="server" CssClass="btn btn-primary" Text="Add Attraction"
                        OnClick="btnAddAttraction_Click" />
                    <div class="clearfix">
                    </div>
                    <asp:Label ID="lblNote" runat="server" CssClass="SuccessMsg" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
