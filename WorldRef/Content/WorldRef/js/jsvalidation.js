//Phone Number validation
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

// validation for non numeric
function validateNonNumericKeyPress(evt)
{

    var charCode = (evt.which) ? evt.which : event.keyCode;

    if ((charCode >= 65 && charCode <= 90)) {

        return true;
    }

    if (charCode >= 97 && charCode <= 122) {
        return true;
    }
    if (charCode == 32) {
        return true;
    }
    return false;
}
//function numberOnlyExample() {
//    if ((event.keyCode >= 65 && event.keyCode <= 90)) {

//        return true;
//    }

//    return false;
//}
//validation for float 
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function validateFloatKeyPress( evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;

    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57)) {

        return false;
    }

    if (charCode == 46 && el.value.indexOf(".") !== -1) {
        return false;
    }

    return true;
}


