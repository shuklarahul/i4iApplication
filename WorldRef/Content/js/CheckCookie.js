﻿function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

function checkCookie() {
    var user = getCookie("UserId");
    if (user != "") {
          return true;
        //$.ajax({
        //    type: "POST",
        //    url: '/WorldRef/CheckUserIdAjax',
        //    data: JSON.stringify({ "projectId": projectId }),
        //    contentType: 'application/json; charset=utf-8',
        //    success: function (data) {//CommentList                      
        //        if (data == "Fail") {
        //            return false;
        //        }
        //        else {
        //            return true;
        //        }
        //    }
        //});
    }
    else {
        return false;
    }

}

function validateEmail1(email) {
    var reg = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    if (reg.test(email)) {
        return true;
    }
    else {
        return false;
    }
}

function CommonUserSignUp() {
    //   var projectId = document.getElementById("projectId").value;
    var organization = document.getElementById("OrganizationReg").value;
    var contactNo = document.getElementById("ContactReg").value;
    var email = document.getElementById("emailReg").value;
    var CPerson = document.getElementById("CpersonReg").value;//Comment
    var Comment = document.getElementById("PasswordRegGU").value;
    var files = document.getElementById("guserPhoto").files;
    if (files.length > 0) {

        alert("111");
        if (window.FormData !== undefined) {

            alert("333");
            var data = new FormData();
            for (var x = 0; x < files.length; x++) {
                alert("4444");
                data.append("file" + x, files[x]);
            }
        }
    }
    alert(data);

    //var formData = new FormData($('#frmUplaodFileAdd')[0]);
    if (CPerson == null || CPerson == "") {
        alert("Please Enter Name");
        document.getElementById("CpersonReg").focus();
        return false;
    }
    if (organization == null || organization == "") {
        alert("Please Enter Organization Name");
        document.getElementById("OrganizationReg").focus();
        return false;
    }
    if (contactNo == null || contactNo == "") {
        alert("Please Enter Contact Number");
        document.getElementById("ContactReg").focus();
        return false;
    }
    if (email == null || email == "") {
        alert("Please Enter Email");
        document.getElementById("emailReg").focus();
        return false;
    }
    if (Comment == null || Comment == "") {
        alert("Please Enter Password");
        document.getElementById("PasswordRegGU").focus();
        return false;
    }
    // var val = ValidateEmail(email);
    var val = validateEmail1(document.getElementById("emailReg").value);
    if (val == false) {
        return false;
    }
    $.fancybox.close();
    $.ajax({
        type: "POST",
        url: '/WorldRef/SignUpUser',
        data: JSON.stringify({ "emailId": email, "ContactNumber": contactNo, "CompanyName": organization, "CPerson": CPerson, "UserName": email, "password": Comment}),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            alert(data);
            $.fancybox.close();
        }
    });

}

function RecruiterSignUp() {
    //   var projectId = document.getElementById("projectId").value;
    var organization = document.getElementById("txtOrgName").value;
    var contactNo = document.getElementById("txtConNum").value;
    var email = document.getElementById("txtEmailAdd").value;
    var CPerson = document.getElementById("txtRecName").value;//Comment
    var Comment = document.getElementById("txtPasswordReg").value;
  
    if (CPerson == null || CPerson == "") {
        alert("Please Enter Name");
        document.getElementById("txtRecName").focus();
        return false;
    }
    if (organization == null || organization == "") {
        alert("Please Enter Organization Name");
        document.getElementById("txtOrgName").focus();
        return false;
    }
    if (contactNo == null || contactNo == "") {
        alert("Please Enter Contact Number");
        document.getElementById("txtConNum").focus();
        return false;
    }
    if (email == null || email == "") {
        alert("Please Enter Email");
        document.getElementById("txtEmailAdd").focus();
        return false;
    }
    if (Comment == null || Comment == "") {
        alert("Please Enter Password");
        document.getElementById("txtPasswordReg").focus();
        return false;
    }
    // var val = ValidateEmail(email);
    var val = validateEmail1(document.getElementById("txtEmailAdd").value);
    if (val == false) {
        return false;
    }
    $.fancybox.close();
    $.ajax({
        type: "POST",
        url: '/WorldRef/SignUpUser',
        data: JSON.stringify({ "emailId": email, "ContactNumber": contactNo, "CompanyName": organization, "CPerson": CPerson, "UserName": email, "password": Comment }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            alert(data);
            $.fancybox.close();
        }
    });

}

function IndustryList() {
    $.ajax({
        url: "/WorldRef/GetAllIndustry",
        type: 'POST',
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            var Result = "";
            $.each(response, function (id, model) {
                Result += ' <input type="checkbox" name="' + model.CustomerIndustryType + '" class="filter-select" onchange="CallCountrySearchHome()" value="Only India">&nbsp;&nbsp;' + model.CustomerIndustryType + '<br>';
            });
            $("#industry").html(Result);
        },
        error: function () {
            alert("Failed! Please try again.");
        }
    });
}

function checkLikeCookie() {
    var user = getCookie("CUserId");
    if (user != "") {
        return true;
    }
    else {
        return false;
    }

}

function openFancybox() {
    $.fancybox({
        'autoScale': true,
        'transitionIn': 'elastic',
        'transitionOut': 'elastic',
        'speedIn': 500,
        'speedOut': 300,
        'autoDimensions': true,
        'centerOnScroll': true,
        'href': '#inline1'
    });
}

var _validFileExtensions = [".pdf"];
function ValidateSingleInput(oInput) {
    if (oInput.type == "file") {
        var sFileName = oInput.value;
        if (sFileName.length > 0) {
            var blnValid = false;
            for (var j = 0; j < _validFileExtensions.length; j++) {
                var sCurExtension = _validFileExtensions[j];
                if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                    blnValid = true;
                    break;
                }
            }

            if (!blnValid) {
                alert("Sorry, File is invalid, allowed extensions are: " + _validFileExtensions.join(", "));
                oInput.value = "";
                return false;
            }
        }
    }
    return true;
}

var _validFileExtensions1 = [".jpg", ".jpeg", ".bmp", ".png"];
function ValidateSingleInput1(oInput) {
    if (oInput.type == "file") {
        var sFileName = oInput.value;
        if (sFileName.length > 0) {
            var blnValid = false;
            for (var j = 0; j < _validFileExtensions1.length; j++) {
                var sCurExtension = _validFileExtensions1[j];
                if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                    blnValid = true;
                    break;
                }
            }

            if (!blnValid) {
                alert("Sorry,file is invalid, allowed extensions are: " + _validFileExtensions1.join(", "));
                oInput.value = "";
                return false;
            }
        }
    }
    return true;
}
var txtCounter = 1;
var fileCounter = 1;
function AddDynamicTxtbox() {

    var str;
    str = "<div id = 'spacetxt'  style = 'width:100%;display:block;margin-bottom: 27px;'><input class='sign-up-input' style='float:left;margin-bottom:10px;width: 94%;position: relative;' type='text' name='txt" + txtCounter + "' id='txt" + txtCounter + "' /><img src='/Content/WorldRef/images/delete.png' id = 'img" + txtCounter + "' onclick = 'return DeleteCompanyTab(" + txtCounter + ")' title='Delete' /> </div>";
    var newApnd = document.getElementById("TextBoxContain");
    $("#TextBoxContain").append(str);
    txtCounter++;
}

function DeleteCompanyTab(imgid) {
    // document.getElementById("TextBoxContainer").removeChild(imgid.parentNode);
    var imgId = "img" + (imgid).toString();
    var imgId1 = "img" + (imgid - 1).toString();
    var child = document.getElementById(imgId);
    var parent = document.getElementById('TextBoxContain');
    parent.removeChild(child.parentNode);
    var child1 = document.getElementById("txt" + imgid);
    parent.removeChild(child1.parentNode);
    if (imgid != 0) {
        document.getElementById(imgId1).style.display = "block";
        document.getElementById('ContentPlaceHolder1_hdniValue').value = imgid;
    }
    else {
        document.getElementById('ContentPlaceHolder1_hdniValue').value = 0;
    }
    //   txtCounter--;
    return false;

}

function GetProjectImageLikedUser(projectId,projectImageId) {
    $("#LikedUser").html("");
    $.ajax({
        type: "POST",
        url: '/WorldRef/GetProjectImageLikeUser',
        data: JSON.stringify({ "projectId": projectId, "ProjectImageId": projectImageId }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {//CommentList  
            var Result = "<div class='profile_part2 popBoxWidth_fix border_design'><h4 class='fancyCu_h4'>Published Testimonial</h4>";
            var Result = "<p class='like-title'>Users Who Liked It</p><ul style='text-align=center;color=black'>";
            for (i = 0; i < data.length; i++) {
                if (i % 2 == 0) {
                    Result += '<li class="mbColor2">';
                }
                else {
                    Result += '<li class="mbColor1">';
                }

                Result += "<span style='font-weight:bold;color: green'>" + data[i];
            }
            //  Result += "<li><input type='text' id=txt" + projectId + "></input><input type='button' value='Add Comment' onclick='AddComment(" + projectId + ")'></input></li>";
			Result +="</ul>";           
		   $("#LikedUser").html(Result);
            $.fancybox("#LikedUser");
        },
        error: function () {
            alert("Failed! Please try again.");
        }
    });
}