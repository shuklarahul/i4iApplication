$(document).ready(function () {

    var oTable = $('#myDataTable').dataTable({
    	"bServerSide": true,
    	"sAjaxSource": "Home/AjaxHandler", 
    	"bProcessing": true,
    	"aoColumns": [
                        {   "sName": "ID",
                            "bSearchable": false,
                            "bSortable": false,
                            "fnRender": function (oObj) {
                                return '<a href=\"Company/Details/' + oObj.aData[0] + '\">View</a>';
                            }
                        },
			            { "sName": "CustomerName" },
			            { "sName": Projectname},
			            { "sName": "ProjectType" }
   { "sName": "CustomerNeeds" }
   { "sName": "OfferSubmissionDate" }
		            ]
    });
});
