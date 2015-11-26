function SignUpFormValidation() {

    if ($("#DropdownType").val() == "0") {
        alert("Please Select Type");
        document.getElementById("DropdownType").focus();
        return false;
    }

    if ($("#DropdownType").val() == "Others") {
        var y1;
        y1 = document.getElementById("OtherType").value;
        if (y1 == null || y1 == "") {
            alert("Please enter Type ");
            document.getElementById("OtherType").focus();
            return false;
        }
    }
    var y;
    y = document.getElementById("Name").value;
    if (y == null || y == "") {
        alert("Please enter  Name");
        document.getElementById("Name").focus();
        return false;
    }
	 var Contain = "";
                $("#TextBoxContain :text").each(function () {
                    Contain += $(this).val();
                });
        y = document.getElementById("Industry").value;
        if ((y == null || y == "" )&& Contain == "") {
            alert("Please Select  Industry or Add your Industry");
            document.getElementById("Industry").focus();
            return false;
        }
    var y;
    y = document.getElementById("Email").value;
    if (y == null || y == "") {
        alert("Please enter Email");
        document.getElementById("Email").focus();
        return false;
    }
    var y;
    y = document.getElementById("AlternateEmail").value;
    if (y == null || y == "") {
        alert("Please Enter Alternate Email");
        document.getElementById("Email").focus();
        return false;
    }
    var y;
   
    var y;
    y = document.getElementById("ContactNumber").value;
    if (y == null || y == "") {
        alert("Please enter Contact Number");
        document.getElementById("ContactNumber").focus();
        return false;
    }
    if (document.getElementById('Country').selectedIndex == 0) {
        alert("Please Select Country");
        return false;
    }

    var y = document.getElementById("fileUp").value;
    if (y == null || y == "") {
        alert("Please Select Company Profile");
        document.getElementById("fileUp").focus();
        return false;
    }
    var y = document.getElementById("Logo").value;
    if (y == null || y == "") {
        alert("Please Select Company Logo");
        document.getElementById("Logo").focus();
        return false;
    }
}
function UploadDocument() {
    var cookieValue = document.cookie;
    var abd = checkCookie();
    if (abd == false || abd === 'undefined' || abd === "") {
        openFancybox();
    }
    else if (cookieValue == null || cookieValue == 'undefined' || cookieValue == "") {
        openFancybox();
    }
    else {
        window.location.href = '/Uploader/UploadExcel'

    }
}
function CountryList() {
    $.ajax({
        url: "/WorldRef/GetAllCountries",
        type: 'POST',
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            var Result = "";
            $.each(response, function (id, model) {
                Result += ' <input type="checkbox" name="' + model.Country + '" class="filter-select" onchange="CallCountrySearchHome()" value="Only India">&nbsp;&nbsp;' + model.Country + '<br>';
            });
            $("#country").html(Result);
        },
        error: function () {
            alert("Failed! Please try again.");
        }
    });
}



function ShowViewBag() {
    $("#JavascriptResult").hide();
    $("#viewBagResult").show();
}
function HideViewBag() {
    $("#viewBagResult").hide();
    $("#JavascriptResult").show();
}
function getAllSelectedCheckBox() {
  
    var selected = [];
    $('#country input:checked').each(function () {
        selected.push($(this).attr('name'));
    });
    //  alert(selected);
    return selected;
}

function getAllSelectedCheckBoxIndustry() {
    // alert("called1");
    var selected = [];
    $('#industry input:checked').each(function () {
        selected.push($(this).attr('name'));
    });
    //  alert(selected);
    return selected;
}

function CallCountrySearch()
{
    var cookieUserRole = '@ViewBag.cookieUserRole';

    if (cookieUserRole == null || cookieUserRole == "") {
        cookieUserRole = "";
    }

    document.getElementById('BindModel').innerHTML = "";
    document.title = "WorldRef -" + document.getElementById("SearchValue").value;
    y = document.getElementById("SearchValue").value;
    var selectedCountry = [];
    selectedCountry = getAllSelectedCheckBox();
    var selectedIndustry = [];
    selectedIndustry = getAllSelectedCheckBoxIndustry();
    $.ajax({
        url: "/WorldRef/SearchCountryWise",
        type: 'POST',
        data: JSON.stringify({ "searchString": y, "ArrayIndustry": selectedIndustry, "ArrayCountry": selectedCountry }),
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            var Result = '<ul class="projects-slider projectsbysearch">';
            var i = 0;
            $.each(response, function (id, model) {
                var a = 0;
                var x = 0;
                var flag = 0;
                Result += '<li class="project one-fourth">';
                Result += '<div class="galcontainer">';
                var students = model.ListOfImage;
                for (var j = 0; j < students.length; j++) {

                    if (students.length == 1) {
                        if ((students[j].ImagePath.toString().indexOf(".png") > -1) || (students[j].ImagePath.toString().indexOf('.bmp') > -1) || (students[j].ImagePath.toString().indexOf('.jpg') > -1)) {
                            Result += '<div class="galler-projgrid">';
                            Result += '<a href="/uploads/' + students[j].ImagePath + '" class="html5lightbox" data-group="set0" data-width="600" data-height="400"><img src="/uploads/' + students[j].ImagePath + '" class="projgallery"></a><img src="/Content/WorldRef/images/no-image.png" class="projgallery">';
                            Result += '</div>';
                        }
                        else if (students[j].Link == false) {
                            Result += '<div class="galler-projgrid">';
                            Result += '<a href="/uploads/' + students[j].ImagePath + '" data-thumbnail="/uploads/' + students[j].ImagePath + '" class="html5lightbox" data-group="set0" data-width="600" data-height="400"><img src="/uploads/video-play-2.gif" class="projgallery"></a>';
                            Result += '</div>';
                        }
                        else {
                            Result += '<div class="galler-projgrid">';
                            Result += '<a href="' + students[j].ImagePath + '" data-thumbnail="/uploads/' + students[j].ImagePath + '" class="html5lightbox" data-group="set1" data-width="600" data-height="400"><img src="/uploads/video-play-2.gif"> </a>';
                            Result += '</div>';
                        }
                    }

                    else if (students.length == 2) {
                        if (flag == 0) {
                            if ((students[0].ImagePath.toString().indexOf(".png") > -1) || (students[0].ImagePath.toString().indexOf('.bmp') > -1) || (students[0].ImagePath.toString().indexOf('.jpg') > -1) || (students[0].ImagePath.toString().indexOf('.jpeg') > -1)) {
                                Result += '<div class="galler-projgrid">';
                                Result += '<a href="/uploads/' + students[0].ImagePath + '" class="html5lightbox" data-group="set0" data-width="600" data-height="400"><img src="/uploads/' + students[0].ImagePath + '" class="projgallery"> </a>';


                                if ((students[1].ImagePath.toString().indexOf(".png") > -1) || (students[1].ImagePath.toString().indexOf('.bmp') > -1) || (students[1].ImagePath.toString().indexOf('.jpg') > -1) || (students[1].ImagePath.toString().indexOf('.jpeg') > -1)) {
                                    Result += '<a href="/uploads/' + students[1].ImagePath + '" class="html5lightbox" data-group="set0" data-width="600" data-height="400"><img src="/uploads/' + students[1].ImagePath + '" class="projgallery"></a>';

                                }
                                else if (students[1].Link == false) {
                                    Result += '<a href="/uploads/' + students[1].ImagePath + '" data-thumbnail="/uploads/' + students[1].ImagePath + '" class="html5lightbox" data-group="set0" data-width="600" data-height="400"><img src="/uploads/video-play-2.gif" class="projgallery"></a>';
                                }
                                else {
                                    Result += '<a href="' + students[1].ImagePath + '" data-thumbnail="/uploads/' + students[1].ImagePath + '" class="html5lightbox" data-group="set0" data-width="600" data-height="400"><img src="/uploads/video-play-2.gif" class="projgallery"></a>';
                                }
                                Result += '</div>';

                            }
                            if ((students[1].ImagePath.toString().indexOf(".png") > -1) || (students[1].ImagePath.toString().indexOf('.bmp') > -1) || (students[1].ImagePath.toString().indexOf('.jpg') > -1) || (students[1].ImagePath.toString().indexOf('.jpeg') > -1)) {
                                Result += '<a href="/uploads/' + students[1].ImagePath + '" class="html5lightbox" data-group="set0" data-width="600" data-height="400"><img src="/uploads/' + students[1].ImagePath + '" class="projgallery"></a>';

                            }
                            else if (students[1].Link == false) {
                                Result += '<a href="/uploads/' + students[1].ImagePath + '" data-thumbnail="/uploads/' + students[1].ImagePath + '" class="html5lightbox" data-group="set0" data-width="600" data-height="400"><img src="/uploads/video-play-2.gif" class="projgallery"></a>';
                            }
                            else {
                                Result += '<a href="' + students[1].ImagePath + '" data-thumbnail="/uploads/' + students[1].ImagePath + '" class="html5lightbox" data-group="set0" data-width="600" data-height="400"><img src="/uploads/video-play-2.gif" class="projgallery"></a>';
                            }
                        }
                        flag = 1;
                    }
                    else if (students.length > 2) {
                        if (flag == 0) {
                            x = students.length;
                        }
                        if ((students[j].ImagePath.toString().indexOf(".png") > -1) || (students[j].ImagePath.toString().indexOf('.bmp') > -1) || (students[j].ImagePath.toString().indexOf('.jpg') > -1)) {
                            Result += '<div class="galler-projgrid">';
                            Result += '<a href="/uploads/' + students[j].ImagePath + '" class="html5lightbox" data-group="set0" data-width="600" data-height="400"><img src="/uploads/' + students[j].ImagePath + '" class="projgallery"> </a>';


                            if ((students[j + 1].ImagePath.toString().indexOf(".png") > -1) || (students[j + 1].ImagePath.toString().indexOf('.bmp') > -1) || (students[j + 1].ImagePath.toString().indexOf('.jpg') > -1)) {
                                Result += '<a href="/uploads/' + students[j + 1].ImagePath + '" class="html5lightbox" data-group="set0" data-width="600" data-height="400"><img src="/uploads/' + students[j + 1].ImagePath + '" class="projgallery"></a>';

                            }
                            else if (students[j + 1].Link == false) {
                                Result += '<a href="/uploads/' + students[j + 1].ImagePath + '" data-thumbnail="/uploads/' + students[j + 1].ImagePath + '" class="html5lightbox" data-group="set0" data-width="600" data-height="400"><img src="/uploads/video-play-2.gif" class="projgallery"></a>';
                            }
                            else {
                                Result += '<a href="' + students[j + 1].ImagePath + '" data-thumbnail="/uploads/' + students[j + 1].ImagePath + '" class="html5lightbox" data-group="set0" data-width="600" data-height="400"><img src="/uploads/video-play-2.gif" class="projgallery"></a>';
                            }
                            Result += '</div>';
                        }
                        else if (students[j].ImagePath.Link == false) {
                            Result += '<div class="galler-projgrid">';
                            Result += '<a href="/uploads/' + students[j].ImagePath + '" data-thumbnail="/uploads/' + students[j].ImagePath + '" class="html5lightbox" data-group="set0" data-width="600" data-height="400"><img src="/uploads/video-play-2.gif" class="projgallery"></a>';

                            Result += '</div>';
                        }
                        else {
                            Result += '<div class="galler-projgrid">';
                            Result += '<a href="' + students[j].ImagePath + '" data-thumbnail="/uploads/' + students[j].ImagePath + '" class="html5lightbox" data-group="set0" data-width="600" data-height="400"><img src="/uploads/video-play-2.gif" class="projgallery"> </a>';
                            Result += '</div>';
                        }

                        flag = 1;
                    }
                    x = x - 1;
                }
                if (students.length > 2) {
                    Result += '<div class="bx-controls-direction"><div class="bx-prev">Prev</div><div class="bx-next">Next</div></div>';
                }

                if (students.length == 0) {

                    Result += '<div>';
                    Result += '<img src="/Content/WorldRef/images/no-image.png" class="projgallery">';
                    Result += '<img src="/Content/WorldRef/images/no-image.png" class="projgallery noMargin_right">';
                    Result += '</div>';
                }
                Result += '<div class="proj-description responsiveQ">';
                Result += '  <div class="title-hydro"> <div class="search-info"><div class="logomain-box"><img src="/uploads/' + model.CompanyLogo + '" class="onHover_img" /></div>';
                Result += ' <div class="prj-decr"><span class="projectuser-text">Reference Name </span>:  ' + model.ProjectName + ' <br />';
                Result += ' <span class="projectuser-text">By </span>: ' + model.OrganizationName + ' <br />';
                Result += ' <span class="projectuser-text">For </span>:  ' + model.CustomerName + ' <br />';
                Result += ' <span class="projectuser-text">For Industry</span>:  ' + model.CustomerIndustryType + ' <br />';
                Result += ' <span class="projectuser-text">Contribution </span>:  ' + model.Type + ' <br />';
                Result += ' <span class="projectuser-text">Country of Execution </span>:  ' + model.Country + ' <br />';
                Result += ' <span class="projectuser-text">Status </span>:  ' + model.Status + ' <br />';
                Result += ' <span class="projectuser-text">Year of Execution </span>:  ' + model.Year + ' <br /> ';
                if (model.Description == null) {
                    model.Description = "";
                }
                Result += ' <span class="projectuser-text">Description </span>:  ' + model.Description + ' </div> </div>  ';

                var strmake = "SetProjectId('" + model.id + "','" + model.UserType + "')";

                Result += '<div> <span class="projectimg-icon">    <a href="#inline6" onclick="popEx(' + model.id + ')" class="fancybox" data-fancybox-type="iframe"> <img src="/Content/WorldRef/images/CompanyProfile.png" title="View Company Profile" /> </a></span>';
                Result += '<span class="projectimg-icon"><a href="javascript:void(0)"  onclick="AddLikeOrDisLike(' + model.id + ')"> <img src="/Content/WorldRef/images/like-20.png" title="Like This"></a> ';
                Result += '<label title="Likes" class="like-color" id=' + model.id + 'Like onclick="GetProjectLikedUser(' + model.id + ')">' + model.TotalLikes + '</label></span>';

                Result += '<span class="projectimg-icon"> <a href="#inline4" class="fancybox" onclick="GetAllCommentsOfProject(' + model.id + ')"><img src="/Content/WorldRef/images/Comment.png" title="Review" /></a> </span>';

                Result += ' <span class="projectimg-icon"><a href="#inline5" class="fancybox" onclick="' + strmake + '"><img src="/Content/WorldRef/images/connect.png" title="Connect with the ' + model.UserType + '"> </a> </span>';

                Result += '<span class="projectimg-icon"><a href="javascript:void(0)" onclick="AddCredit(' + model.id + ')"><img src="/Content/images/creditm.png" alt="" title="Mark your Contribution" /></a></span>';

                Result += '<span class="projectimg-icon"><a href="#" class="fancybox"><img src="../Content/images/i4iicon.png" alt="" title="i4iKnowledge" /></a></span><span class="projectimg-icon"><a href="#inline6" onclick="popExCertificates(' + model.id + ')" class="fancybox" data-fancybox-type="iframe"><img src="/Content/WorldRef/images/it-08.png" title="View Certificate" /></a></span></div>';

                Result += '<div class="rating-star rating-mainbox"><div class=""><div class="i4i-rating" style="width: 32%; float: left; text-align: center;"><span style="margin-right: 4%; display: block; font-size: 11px; color: #898989;">Price Rating</span><label for="pricerating" style="  display: block;  margin-top: 6px;" id="txt"' + model.id + '>' + model.Rating + '</label></div><div style="width: 33%; float: left; border-right: 1px solid #DBDBDB; text-align: center; border-left: 1px solid #D7D7D7;"><span style="margin-right: 4%; display: block; font-size: 11px; color: #898989;">Quality Rating</span><label for="QualityRating" style="  display: block;  margin-top: 6px;" id="txt"' + model.id + '>' + model.QualityRating + ' /10</label></div><div style="width: 35%; float: left; text-align: center;"><span style="margin-right: 4%; display: block; font-size: 11px; color: #898989;">Reliability Rating</span><label for="relibility" style="  display: block;  margin-top: 6px;" id="txt"' + model.id + '>' + model.UserRating + ' /10</label></div></div></div>';

                Result += '<div class="user_t"><div class="user_innerbox"><span class="projectuser-text" style="margin-top:0px;">Uploaded by</span>  : ' + model.UserType + ' </div></div></div></div></div>';

                console.log(cookieUserRole);
                if (cookieUserRole == "P") {
                    if (model.ListOfApprovedCredits.length > 0) {
                        Result += '<div class="creditDes"><h4>Contributors</h4>';
                        for (var a = 0; a < (model.ListOfApprovedCredits).length; a++) {
                            Result += '<div class="image_tn src-img"><img id="" title="' + model.ListOfApprovedCredits[a].firstName + ' ' + model.ListOfApprovedCredits[a].Designation + ' ' + model.ListOfApprovedCredits[a].Industry + '  " src="' + model.ListOfApprovedCredits[a].picUrl + '"></div>';
                        }
                        Result += '</div>';
                    }
                }

                Result += '</div></div></div></div></li>';
                i++;
            });

            Result += '</ul>';
            $("#BindModel").html(Result);
            $(".html5lightbox").html5lightbox();
            var count = "";
            document.getElementById('viewBagResult').innerHTML = "";
            count += '<li class="searchmenu " id="countRecord">Total Results Found: ' + i + '</li>';
            $("#viewBagResult").html(count);
            $('.projectsbysearch').show();
            $(".html5lightbox").html5lightbox();

            var currentIndex = 0,
    items = $('.galcontainer .galler-projgrid'),
    itemAmt = items.length;
            cycleItems();
            function cycleItems() {

                var item = $('.galcontainer .galler-projgrid').eq(currentIndex);
                items.hide();

                item.css('display', 'inline-block');
            }
            $('.bx-next').click(function () {
                currentIndex += 1;
                if (currentIndex > itemAmt - 1) {
                    currentIndex = 0;
                }
                cycleItems();
            });
            $('.bx-prev').click(function () {

                currentIndex -= 1;
                if (currentIndex < 0) {
                    currentIndex = itemAmt - 1;
                }
                cycleItems();
            });
        },
        error: function () {
            alert("Failed! Please try again.");
        }
    });
}



function BindDataForFirstTime() {
    y = document.getElementById("SearchValue").value;
    $.ajax({
        url: "/WorldRef/Search",
        type: 'POST',
        data: JSON.stringify({ "searchString": y }),
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            var Result = "";
            $.each(response, function (id, model) {
                Result += '<div>';
                // Result += '  <div class="title"><a href="/WorldRef/ParticularProject?projectId" >' + model.ProjectName + '</a></div>';
                Result += '  <div class="title"><a href="/WorldRef/ParticularProject?projectId"' + model.id + ' >' + model.ProjectName + '</a></div>';
                Result += ' <div class="sourcetitle">' + model.OrganizationName + '</div>';
                Result += ' <div class="ponsoredlink">' + model.Country + '</div>';
                Result += '</div> <br />';
            });
            $("#BindModel").html(Result);
        },
        error: function () {
            alert("Failed! Please try again.");
        }
    });
}

function returnNextImage(projectId, index) {
    index = parseInt(index) + 1;
    $.ajax({
        type: "POST",
        url: '/WorldRef/Image',
        data: JSON.stringify({ "projectId": projectId, "index": index, "ButtonType": "next" }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.ImagePath != "" && data.ImagePath != null && data.ImagePath != undefined) {
                if (data.Link == false) {
                    var splitdata = data.ImagePath.split('.');
                    if (splitdata[1] == "mp4" || splitdata[1] == "avi" || splitdata[1] == "3gp") {
                        $('#' + projectId).attr('src', "/uploads/video-play-2.gif");
                    }
                    else {
                        $('#' + projectId).attr('src', "/uploads/" + data.ImagePath);
                    }
                }
                else {
                    $('#' + projectId).attr('src', "/uploads/video-play-2.gif");
                }
                $('#' + projectId + 'next').attr('onclick', 'returnNextImage(' + projectId + ',' + data.ImgIndex + ')');
                $('#' + projectId + 'prev').attr('onclick', "returnPrevImage(" + projectId + "," + data.ImgIndex + ");");
            }
        }
    });
}

function returnPrevImage(projectId, index) {
    index = parseInt(index) - 1;
    $.ajax({
        type: "POST",
        url: '/WorldRef/Image',
        data: JSON.stringify({ "projectId": projectId, "index": index, "ButtonType": "prev" }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.ImagePath != "" && data.ImagePath != null && data.ImagePath != undefined) {
                if (data.Link == false) {
                    var splitdata = data.ImagePath.split('.');
                    if (splitdata[1] == "mp4" || splitdata[1] == "avi" || splitdata[1] == "3gp") {
                        $('#' + projectId).attr('src', "/uploads/video-play-2.gif");
                    }
                    else {
                        $('#' + projectId).attr('src', "/uploads/" + data.ImagePath);
                    }
                }
                else {
                    $('#' + projectId).attr('src', "/uploads/video-play-2.gif");
                }
                $('#' + projectId + 'next').attr('onclick', "returnNextImage(" + projectId + "," + data.ImgIndex + ")");
                $('#' + projectId + 'prev').attr('onclick', "returnPrevImage(" + projectId + "," + data.ImgIndex + ")");
            }
        }
    });
}

function SetProjectId(id, userType) {
 
    document.getElementById("projectId").value = id;
    
}
function AskCustomer()
{//OrganizationName

    var projectId = document.getElementById("projectId").value;
    var organization = document.getElementById("Organization").value;
    var contactNo = document.getElementById("Contact").value;
    var email = document.getElementById("email").value;
    var CPerson = document.getElementById("CPerson").value;
    var query = document.getElementById("Query").value;

    if (organization == null || organization == "") {
        alert("Please enter  organization Name");
        document.getElementById("Organization").focus();
        return false;
    }
    if (CPerson == null || CPerson == "") {
        alert("Please Enter Contact Person Name");
        document.getElementById("CPerson").focus();
        return false;
    }
    if (contactNo == null || contactNo == "") {
        alert("Please enter contact number");
        document.getElementById("Contact").focus();
        return false;
    }
    if (email == null || email == "") {
        alert("Please enter Email");
        document.getElementById("email").focus();
        return false;
    }
    if (query == null || query == "") {
        alert("Please Enter Query");
        document.getElementById("Query").focus();
        return false;
    }
    // var val = ValidateEmail(email);
    var val = validateEmail1(document.getElementById("email").value);
    if (val == false) {
        return false;
    }
    $.fancybox.close();
    $.ajax({
        type: "POST",
        url: '/WorldRef/Ask',
        data: JSON.stringify({ "emailId": email, "ContactNumber": contactNo, "CompanyName": organization, "ProjectId": projectId, "CPerson": CPerson, "Query": query }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            alert(data);
            $.fancybox.close();
        }
    });

}
function AskI4I() {//OrganizationName
    //   alert(document.getElementById("projectId").value);
    var projectId = document.getElementById("projectId").value;
    var organization = document.getElementById("Organizationi4i").value;
    var contactNo = document.getElementById("Contacti4i").value;
    var email = document.getElementById("emaili4i").value;
    var CPerson = document.getElementById("Cpersoni4i").value;
    var Cdesignation = document.getElementById("Designationi4i").value;

    if (organization == null || organization == "") {
        alert("Please enter organization name");
        document.getElementById("Organizationi4i").focus();
        return false;
    }
    if (CPerson == null || CPerson == "") {
        alert("Please enter contact person");
        document.getElementById("Cpersoni4i").focus();
        return false;
    }
    if (contactNo == null || contactNo == "") {
        alert("Please enter contact number");
        document.getElementById("Contacti4i").focus();
        return false;
    }
    if (email == null || email == "") {
        alert("Please enter emailid");
        document.getElementById("emaili4i").focus();
        return false;
    }
    // var val = ValidateEmail(email);
    var val = validateEmail1(document.getElementById("emaili4i").value);
    if (val == false) {
        return false;
    }
    $.fancybox.close();
    $.ajax({
        type: "POST",
        url: '/WorldRef/Aski4i',
        data: JSON.stringify({ "emailId": email, "ContactNumber": contactNo, "CompanyName": organization, "ProjectId": projectId, "CPerson": CPerson,"designation":Cdesignation}),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            // alert(data);
            if (data != "")
            {
                alert(data);
            }
                
           $.fancybox.close();
        }
    });

}

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function ValidateEmail(mail) {
    if (/^\w+([\.-]?\w+)*@@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(document.getElementById("email").value)) {
        return (true);
    }
    alert("You have entered an invalid email address!");
    document.getElementById("email").focus();
    return (false);
}

function PopUpVideoFrame(projectId) {
    $.ajax({
        type: "POST",
        url: '/WorldRef/_viewImages',
        data: JSON.stringify({ "ProjectId": projectId }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#PartialView").html(data);
        }
    });
}

function OpenLikePopUp(projectId) {
    document.getElementById("projectId").value = projectId;
    document.getElementById("LikesOrDislikes").value = "Like";
}

function OpenDisLikePopUp(projectId) {
    document.getElementById("projectId").value = projectId;
    document.getElementById("LikesOrDislikes").value = "DisLike";
}

function AddLikeOrDisLike(projectId,uId) {
    $.ajax({
        type: "POST",
        url: '/WorldRef/AddLikeDislike',
        data: JSON.stringify({ "ProjectId": projectId,"UserID":uId, "LikeOrDisLike": "Like", "UserIp": "0" }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data == "login") {
                MovetoLoginTab();
              // $.fancybox("#inline111");
            }
            else if (data == "userProject")
            {
                alert("You Can not Like to own project.");
            }
            else if (data == "Success") {
                alert("You have successfully liked.");
            }
            else if (data == "You Already Liked this Project") {
                alert("You Already Liked this Project.");
            }
            else if (data == "Fail") {
                alert("There have been some error. Please try again.");
            }
            else {
                CallCountrySearchHome();
            }
        }
    });

}



// Code for credit 'em
function AddCredit(projectId,uId) {

    document.getElementById("projectId").value = projectId;
    $.ajax({
        type: "POST",
        url: '/WorldRef/CreditThem',
        data: JSON.stringify({ "ProjectId": projectId, "Userid": uId, "UserIp": "0" }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data == "login") {
                $.fancybox("#inline1");
            }
            else if (data == "userProject")
            {
                alert("You can not ask for credit for Contribution on your own project.")
            }
            else if (data == "Success") {
                alert("Your credit request has been sent and is subjected to approval.");
            }
            else if (data == "submitted") {
                alert("Your credit request has been sent and is subjected to approval.");
            }
            else if (data == "AlreadyRequested") {
                alert("You have Already Requested for Credit.");
            }
            else if (data == "Fail") {
                alert("There have been some error. Please try again.");
            }
        }
    });

}


//end code for credit.



function AddReview() {
    var projectId = document.getElementById("projectId").value;
    var organization = document.getElementById("OrganizationRvr").value;
    var contactNo = document.getElementById("ContactRvr").value;
    var email = document.getElementById("emailRvr").value;
    var CPerson = document.getElementById("CpersonRvr").value;//Comment
    var Comment = document.getElementById("Comment").value;

    if (organization == null || organization == "") {
        alert("Please enter  organization Name");
        document.getElementById("OrganizationRvr").focus();
        return false;
    }
    if (CPerson == null || CPerson == "") {
        alert("Please enter  Contact Person");
        document.getElementById("CpersonRvr").focus();
        return false;
    }
    if (contactNo == null || contactNo == "") {
        alert("Please enter contact number");
        document.getElementById("ContactRvr").focus();
        return false;
    }
    if (email == null || email == "") {
        alert("Please enter Email");
        document.getElementById("emailRvr").focus();
        return false;
    }
    if (Comment == null || Comment == "") {
        alert("Please enter Review");
        document.getElementById("Comment").focus();
        return false;
    }
    // var val = ValidateEmail(email);
    var val = validateEmail1(document.getElementById("emailRvr").value);
    if (val == false) {
        return false;
    }
    $.fancybox.close();
    $.ajax({
        type: "POST",
        url: '/WorldRef/Review',
        data: JSON.stringify({ "emailId": email, "ContactNumber": contactNo, "CompanyName": organization, "ProjectId": projectId, "CPerson": CPerson, "Comment": Comment }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            alert(data);
            $.fancybox.close();
        }
    });

}


function GetProjectLikedUser(projectId) {
    $("#LikedUser").html("");
    $.ajax({
        type: "POST",
        url: '/WorldRef/GetProjectLikeUser',
        data: JSON.stringify({ "projectId": projectId }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {//CommentList     
            var Result = "<div class='profile_part2 popBoxWidth_fix border_design'><h4 class='fancyCu_h4'>Published Testimonial</h4>";
            var Result = "<p class='like-title'>Users Who Liked It</p><ul style='text-align=center;color=black;'>";
            for (i = 0; i < data.length; i++) {
                if (i % 2 == 0) {
                    Result += '<li class="mbColor2">';
                }
                else {
                    Result += '<li class="mbColor1">';
                }

                Result += "<span style='font-weight:bold;color: green'>" + data[i];
            }
			 Result +="</ul>";
            $("#LikedUser").html(Result);
            $.fancybox("#LikedUser");
        },
        error: function () {
            alert("Failed! Please try again.");
        }
    });
}

function CancelLike() {
    $.fancybox.close();
}