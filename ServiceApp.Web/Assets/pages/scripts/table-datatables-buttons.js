var TableDatatablesButtons = function () {

    var initTableCustomerServiceRequests = function () {
        var table = $('#dvCustomerServiceRequests');

        var oTable1 = table.dataTable({

            // Internationalisation. For more info refer to http://datatables.net/manual/i18n
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": "No entries found",
                "infoFiltered": "(filtered1 from _MAX_ total entries)",
                "lengthMenu": "_MENU_ entries",
                "search": "Search:",
                "zeroRecords": "No matching records found"
            },

            // Or you can use remote translation file
            //"language": {
            //   url: '//cdn.datatables.net/plug-ins/3cfcc339e89/i18n/Portuguese.json'
            //},


            buttons: [
                //{ extend: 'print', className: 'btn dark btn-outline' },
                //{ extend: 'copy', className: 'btn red btn-outline' },
                { extend: 'pdf', className: 'btn green btn-outline', title: "Dashboard Report" },
                { extend: 'excel', className: 'btn green btn-outline', title: "Dashboard Report" },
                //{ extend: 'csv', className: 'btn purple btn-outline ' },
                //{ extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            ],

            // setup responsive extension: http://datatables.net/extensions/responsive/
            responsive: true,

            //"ordering": false, disable column ordering 
            //"paging": false, disable pagination

            "order": [
                [0, 'asc']
            ],

            "lengthMenu": [
                [5, 10, 15, 20, -1],
                [5, 10, 15, 20, "All"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,

            "dom": "<'row' <'col-md-3 col-sm-12'B><'col-md-4 col-sm-12'l><'col-md-5 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        });

    }

    var initTableEngineerServiceRequests = function () {
        var table1 = $('#dvEngineerServiceRequests');

        var oTable = table1.dataTable({

            // Internationalisation. For more info refer to http://datatables.net/manual/i18n
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": "No entries found",
                "infoFiltered": "(filtered1 from _MAX_ total entries)",
                "lengthMenu": "_MENU_ entries",
                "search": "Search:",
                "zeroRecords": "No matching records found"
            },

            // Or you can use remote translation file
            //"language": {
            //   url: '//cdn.datatables.net/plug-ins/3cfcc339e89/i18n/Portuguese.json'
            //},


            buttons: [
                //{ extend: 'print', className: 'btn dark btn-outline' },
                //{ extend: 'copy', className: 'btn red btn-outline' },
                { extend: 'pdf', className: 'btn green btn-outline', title:"Dashboard Report" },
                { extend: 'excel', className: 'btn green btn-outline', title: "Dashboard Report" },
                //{ extend: 'csv', className: 'btn green btn-outline ' },
                //{ extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            ],

            // setup responsive extension: http://datatables.net/extensions/responsive/
            responsive: true,

            //"ordering": false, disable column ordering 
            //"paging": false, disable pagination

            "order": [
                [0, 'asc']
            ],

            "lengthMenu": [
                [5, 10, 15, 20, -1],
                [5, 10, 15, 20, "All"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,

            "dom": "<'row' <'col-md-3 col-sm-12'B><'col-md-4 col-sm-12'l><'col-md-5 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        });
    }

    var initTabledvAdminEngineerInfo = function () {
        var tbldvAdminEngineerInfo = $('#dvAdminEngineerInfo');

        var oTableAdminEngineerInfo = tbldvAdminEngineerInfo.dataTable({

            // Internationalisation. For more info refer to http://datatables.net/manual/i18n
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": "No entries found",
                "infoFiltered": "(filtered1 from _MAX_ total entries)",
                "lengthMenu": "_MENU_ entries",
                "search": "Search:",
                "zeroRecords": "No matching records found"
            },

            // Or you can use remote translation file
            //"language": {
            //   url: '//cdn.datatables.net/plug-ins/3cfcc339e89/i18n/Portuguese.json'
            //},


            buttons: [
                //{ extend: 'print', className: 'btn dark btn-outline' },
                //{ extend: 'copy', className: 'btn red btn-outline' },
                { extend: 'pdf', className: 'btn green btn-outline', title: "Engineer Info Report" },
                { extend: 'excel', className: 'btn green btn-outline', title: "Engineer Info Report" },
                //{ extend: 'csv', className: 'btn green btn-outline ' },
                //{ extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            ],

            // setup responsive extension: http://datatables.net/extensions/responsive/
            //responsive: true,

            //"ordering": false, disable column ordering 
            //"paging": false, disable pagination

            "order": [
                [0, 'asc']
            ],

            "lengthMenu": [
                [5, 10, 15, 20, -1],
                [5, 10, 15, 20, "All"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,

            "dom": "<'row' <'col-md-3 col-sm-12'B><'col-md-4 col-sm-12'l><'col-md-5 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        });
    }

    var initTabledvAdminEngineerDetails = function () {
        var tbldvAdminEngineerDetails = $('#dvAdminEngineerDetails');

        var oTabledvAdminEngineerDetails = tbldvAdminEngineerDetails.dataTable({

            // Internationalisation. For more info refer to http://datatables.net/manual/i18n
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": "No entries found",
                "infoFiltered": "(filtered1 from _MAX_ total entries)",
                "lengthMenu": "_MENU_ entries",
                "search": "Search:",
                "zeroRecords": "No matching records found"
            },

            // Or you can use remote translation file
            //"language": {
            //   url: '//cdn.datatables.net/plug-ins/3cfcc339e89/i18n/Portuguese.json'
            //},


            buttons: [
                //{ extend: 'print', className: 'btn dark btn-outline' },
                //{ extend: 'copy', className: 'btn red btn-outline' },
                { extend: 'pdf', className: 'btn green btn-outline', title: "Engineer Details Report" },
                { extend: 'excel', className: 'btn green btn-outline', title: "Engineer Details Report" },
                //{ extend: 'csv', className: 'btn green btn-outline ' },
                //{ extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            ],

            // setup responsive extension: http://datatables.net/extensions/responsive/
            responsive: true,

            //"ordering": false, disable column ordering 
            //"paging": false, disable pagination

            "order": [
                [0, 'asc']
            ],

            "lengthMenu": [
                [5, 10, 15, 20, -1],
                [5, 10, 15, 20, "All"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,

            "dom": "<'row' <'col-md-3 col-sm-12'B><'col-md-4 col-sm-12'l><'col-md-5 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        });
    }

    var initTabledvAdminCustomerDetails = function () {
        var tbldvAdminCustomerDetails = $('#dvAdminCustomerDetails');

        var oTabledvAdminCustomerDetails = tbldvAdminCustomerDetails.dataTable({

            // Internationalisation. For more info refer to http://datatables.net/manual/i18n
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": "No entries found",
                "infoFiltered": "(filtered1 from _MAX_ total entries)",
                "lengthMenu": "_MENU_ entries",
                "search": "Search:",
                "zeroRecords": "No matching records found"
            },

            // Or you can use remote translation file
            //"language": {
            //   url: '//cdn.datatables.net/plug-ins/3cfcc339e89/i18n/Portuguese.json'
            //},


            buttons: [
                //{ extend: 'print', className: 'btn dark btn-outline' },
                //{ extend: 'copy', className: 'btn red btn-outline' },
                { extend: 'pdf', className: 'btn green btn-outline', title: "Customer Details Report" },
                { extend: 'excel', className: 'btn green btn-outline', title: "Customer Details Report" },
                //{ extend: 'csv', className: 'btn green btn-outline ' },
                //{ extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            ],

            // setup responsive extension: http://datatables.net/extensions/responsive/
            responsive: true,

            //"ordering": false, disable column ordering 
            //"paging": false, disable pagination

            "order": [
                [0, 'asc']
            ],

            "lengthMenu": [
                [5, 10, 15, 20, -1],
                [5, 10, 15, 20, "All"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,

            "dom": "<'row' <'col-md-3 col-sm-12'B><'col-md-4 col-sm-12'l><'col-md-5 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        });
    }

    

    var initTabledvAdminSRMgtDetails = function () {
        var tbldvAdminSRMgtDetails = $('#dvAdminSRMgtDetails');

        var oTabledvAdminSRMgtDetails = tbldvAdminSRMgtDetails.dataTable({

            // Internationalisation. For more info refer to http://datatables.net/manual/i18n
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": "No entries found",
                "infoFiltered": "(filtered1 from _MAX_ total entries)",
                "lengthMenu": "_MENU_ entries",
                "search": "Search:",
                "zeroRecords": "No matching records found"
            },

            // Or you can use remote translation file
            //"language": {
            //   url: '//cdn.datatables.net/plug-ins/3cfcc339e89/i18n/Portuguese.json'
            //},


            buttons: [
                //{ extend: 'print', className: 'btn dark btn-outline' },
                //{ extend: 'copy', className: 'btn red btn-outline' },
                { extend: 'pdf', className: 'btn green btn-outline', title: "SR Management Report" },
                { extend: 'excel', className: 'btn green btn-outline', title: "SR Management Report" },
                //{ extend: 'csv', className: 'btn green btn-outline ' },
                //{ extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            ],

            // setup responsive extension: http://datatables.net/extensions/responsive/
            responsive: true,

            //"ordering": false, disable column ordering 
            //"paging": false, disable pagination

            "order": [
                [0, 'asc']
            ],

            "lengthMenu": [
                [5, 10, 15, 20, -1],
                [5, 10, 15, 20, "All"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,

            "dom": "<'row' <'col-md-3 col-sm-12'B><'col-md-4 col-sm-12'l><'col-md-5 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        });
    }

    var initTabledvAdminSROpenDetails = function () {
        var tbldvAdminSROpenDetails = $('#dvAdminSROpenDetails');

        var oTabledvAdminSROpenDetails = tbldvAdminSROpenDetails.dataTable({

            // Internationalisation. For more info refer to http://datatables.net/manual/i18n
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": "No entries found",
                "infoFiltered": "(filtered1 from _MAX_ total entries)",
                "lengthMenu": "_MENU_ entries",
                "search": "Search:",
                "zeroRecords": "No matching records found"
            },

            // Or you can use remote translation file
            //"language": {
            //   url: '//cdn.datatables.net/plug-ins/3cfcc339e89/i18n/Portuguese.json'
            //},


            buttons: [
                //{ extend: 'print', className: 'btn dark btn-outline' },
                //{ extend: 'copy', className: 'btn red btn-outline' },
                { extend: 'pdf', className: 'btn green btn-outline', title: "SR Open Report" },
                { extend: 'excel', className: 'btn green btn-outline', title: "SR Open Report" },
                //{ extend: 'csv', className: 'btn green btn-outline ' },
                //{ extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            ],

            // setup responsive extension: http://datatables.net/extensions/responsive/
            responsive: true,

            //"ordering": false, disable column ordering 
            //"paging": false, disable pagination

            "order": [
                [0, 'asc']
            ],

            "lengthMenu": [
                [5, 10, 15, 20, -1],
                [5, 10, 15, 20, "All"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,

            "dom": "<'row' <'col-md-3 col-sm-12'B><'col-md-4 col-sm-12'l><'col-md-5 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        });
    }

    var initTabledvAdminSRAssignedDetails = function () {
        var tbldvAdminSRAssignedDetails = $('#dvAdminSRAssignedDetails');

        var oTabledvAdminSRAssignedDetails = tbldvAdminSRAssignedDetails.dataTable({

            // Internationalisation. For more info refer to http://datatables.net/manual/i18n
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": "No entries found",
                "infoFiltered": "(filtered1 from _MAX_ total entries)",
                "lengthMenu": "_MENU_ entries",
                "search": "Search:",
                "zeroRecords": "No matching records found"
            },

            // Or you can use remote translation file
            //"language": {
            //   url: '//cdn.datatables.net/plug-ins/3cfcc339e89/i18n/Portuguese.json'
            //},


            buttons: [
                //{ extend: 'print', className: 'btn dark btn-outline' },
                //{ extend: 'copy', className: 'btn red btn-outline' },
                { extend: 'pdf', className: 'btn green btn-outline', title: "SR Assigned Report" },
                { extend: 'excel', className: 'btn green btn-outline', title: "SR Assigned Report" },
                //{ extend: 'csv', className: 'btn green btn-outline ' },
                //{ extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            ],

            // setup responsive extension: http://datatables.net/extensions/responsive/
            responsive: true,

            //"ordering": false, disable column ordering 
            //"paging": false, disable pagination

            "order": [
                [0, 'asc']
            ],

            "lengthMenu": [
                [5, 10, 15, 20, -1],
                [5, 10, 15, 20, "All"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,

            "dom": "<'row' <'col-md-3 col-sm-12'B><'col-md-4 col-sm-12'l><'col-md-5 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        });
    }

    
    var initTabledvAdminSRClosedDetails = function () {
        var tbldvAdminSRClosedDetails = $('#dvAdminSRClosedDetails');

        var oTabledvAdminSRClosedDetails = tbldvAdminSRClosedDetails.dataTable({

            // Internationalisation. For more info refer to http://datatables.net/manual/i18n
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": "No entries found",
                "infoFiltered": "(filtered1 from _MAX_ total entries)",
                "lengthMenu": "_MENU_ entries",
                "search": "Search:",
                "zeroRecords": "No matching records found"
            },

            // Or you can use remote translation file
            //"language": {
            //   url: '//cdn.datatables.net/plug-ins/3cfcc339e89/i18n/Portuguese.json'
            //},


            buttons: [
                //{ extend: 'print', className: 'btn dark btn-outline' },
                //{ extend: 'copy', className: 'btn red btn-outline' },
                { extend: 'pdf', className: 'btn green btn-outline', title: "SR Closed Report" },
                { extend: 'excel', className: 'btn green btn-outline', title: "SR Closed Report" }
                //{ extend: 'csv', className: 'btn green btn-outline ' },
                //{ extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            ],

            // setup responsive extension: http://datatables.net/extensions/responsive/
            responsive: true,

            //"ordering": false, disable column ordering 
            //"paging": false, disable pagination

            "order": [
                [0, 'asc']
            ],

            "lengthMenu": [
                [5, 10, 15, 20, -1],
                [5, 10, 15, 20, "All"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,

            "dom": "<'row' <'col-md-3 col-sm-12'B><'col-md-4 col-sm-12'l><'col-md-5 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        });
    }

    return {

        //main function to initiate the module
        init: function () {

            if (!jQuery().dataTable) {
                return;
            }

            initTableCustomerServiceRequests();
            initTableEngineerServiceRequests();
            initTabledvAdminEngineerInfo();
            initTabledvAdminEngineerDetails();
            initTabledvAdminCustomerDetails();

            initTabledvAdminSRMgtDetails();
            initTabledvAdminSROpenDetails();
            initTabledvAdminSRAssignedDetails();
            initTabledvAdminSRClosedDetails();
        }

    };

}();

jQuery(document).ready(function () {
    TableDatatablesButtons.init();
});