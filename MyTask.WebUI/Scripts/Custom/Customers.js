// Defining angularjs module
var app = angular.module('customerModule', []);

// Defining angularjs Controller and injecting CustomersService
app.controller('customerCtrl', function ($scope, $http, CustomersService) {

    $scope.customersData = null;
    // Fetching records from the factory created at the bottom of the script file
    CustomersService.GetAllRecords().then(function (d) {
        $scope.customersData = d.data; // Success
    }, function () {
        alert('Unable to Get Customers Data !!!'); // Failed
    });

    $scope.total = function () {
        var total = 0;
        angular.forEach($scope.customersData, function (item) {
            total++;
        });
        return total;
    };
    $scope.Customer = {
        ID:'',
        Name:'',
        Details:'',
        Address:'',
        Contact:'',
        IsActive:'',
        CreatedBy:'',
        CreatedOn:'',
        ModifiedBy:'',
        ModifiedOn: '',

        //arrays for storing CustomerNumber stuff
        CustomerNumbers: [],
    };
    console.log('Customer lalala' + JSON.stringify($scope.Customer));

    $scope.CustomerNumber = {
        ID:0,
        NumberValue: '',
        NumberDetail: ''
    }

    $scope.addNewNumber = function () {

        console.log('i am inside addNewNumber');

        if (angular.isDefined($scope.NewNumberValue) && $scope.NewNumberValue != '' && $scope.NewNumberDetail != '') {
            // ADD A NEW ELEMENT.
            $scope.Customer.CustomerNumbers.push({
                ID: $scope.Customer.CustomerNumbers.length+1,
                NumberValue: $scope.NewNumberValue,
                NumberDetail: $scope.NewNumberDetail
            });

            // CLEAR THE FIELDS.
            $scope.NewNumberValue = '';
            $scope.NewNumberDetail = '';
        }

        console.log('i am inside after newaddnumber' + JSON.stringify($scope.Customer));

    };

    $scope.removeNumber = function (index) {
        $scope.Customer.CustomerNumbers.splice(index, 1);
        console.log('i am inside remove number' + index);

    };

    $scope.addNumber = function () {
        console.log('i am inside add number');
        $scope.Customer.CustomerNumbers.push({
            ID: 0,
            NumberValue: '',
            NumberDetail: ''
        });
        console.log('i pushed a new customer number' + JSON.stringify($scope.Customer));
    };

    // Reset product details
    $scope.clear = function () {
        $scope.Customer.ID = '',
        $scope.Customer.Name = '',
        $scope.Customer.Details = '',
        $scope.Customer.Address = '',
        $scope.Customer.Contact = '',
        $scope.Customer.IsActive = '',
        $scope.Customer.CreatedBy = '',
        $scope.Customer.CreatedOn = '',
        $scope.Customer.ModifiedBy = '',
        $scope.Customer.ModifiedOn = '',

        $scope.Customer.CustomerNumbers = [],
        console.log('Customer Cleared ' + JSON.stringify($scope.Customer));
    };

    //Add New Item
    $scope.save = function () {
        if ($scope.Customer.Name != "" &&
            $scope.Customer.Details != "" &&
            $scope.Customer.Address != "" &&
            $scope.Customer.Contact != "" &&
            $scope.Customer.IsActive != "") {

            console.log('i am inside save func' + JSON.stringify($scope.Customer));

            // or you can call Http request using $http
            $http({
                method: 'POST',
                url: '../api/Customers/',
                data: JSON.stringify($scope.Customer)
            }).then(function successCallback(response) {
                // this callback will be called asynchronously
                // when the response is available
                $scope.customersData.push(response.data);
                $scope.clear();
                alert("Customer Added Successfully !!!");
            }, function errorCallback(response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
                alert("Error : " + response.data.ExceptionMessage);
            });
        }
        else {
            alert('Please Enter All the Values !!');
        }
    };

    // Edit product details
    $scope.edit = function (data) {
        console.log('i am inside edit() + '  + JSON.stringify(data.CustomerNumbers));
        $scope.Customer = { ID: data.ID, Name: data.Name, Details: data.Details, Address: data.Address, Contact: data.Contact, CustomerNumbers: data.CustomerNumbers };
    };

    // Cancel product details
    $scope.cancel = function () {
        $scope.clear();

        console.log('i am inside cancel func' + JSON.stringify($scope.Customer));
    };

    // Update product details
    $scope.update = function () {
        if ($scope.Customer.ID != "" &&
            $scope.Customer.Name != "" &&
            $scope.Customer.Details != "" &&
            $scope.Customer.Address != "" &&
            $scope.Customer.Contact != "" &&
            $scope.Customer.IsActive != "") {

            console.log('i am inside update funcr ' + JSON.stringify($scope.Customer));

            $http({
                method: 'PUT',
                url: '../api/Customers/' + $scope.Customer.ID,
                data: JSON.stringify($scope.Customer)
            }).then(function successCallback(response) {
                //$scope.customersData = response.data; this line is causing all data from UI to hide bcz api is not returning any response after updating. need to correct it

                $scope.customersData = null;
                // Fetching records from the factory created at the bottom of the script file
                CustomersService.GetAllRecords().then(function (d) {
                    $scope.customersData = d.data; // Success
                }, function () {
                    alert('Unable to Get Customers Data !!!'); // Failed
                });

                $scope.clear();
                alert("Customer Updated Successfully !!!");
            }, function errorCallback(response) {
                alert("Error : " + response.data.ExceptionMessage);
            });
        }
        else {
            alert('Please Enter All the Values !!');
        }
    };

    // Delete product details
    $scope.delete = function (index) {

        console.log('i am inside delete funcr' + JSON.stringify($scope.Customer));


        $http({
            method: 'DELETE',
            url: '../api/Customers/' + $scope.customersData[index].ID
        }).then(function successCallback(response) {
            $scope.customersData.splice(index, 1);
            alert("Customer Deleted Successfully !!!");
        }, function errorCallback(response) {
            alert("Error : " + response.data.ExceptionMessage);
        });
    };

});

// Here I have created a factory which is a popular way to create and configure services.
// You may also create the factories in another script file which is best practice.

app.factory('CustomersService', function ($http) {
    var fac = {};
    fac.GetAllRecords = function () {
        return $http.get('../api/Customers/');
    };

    console.log('i am inside Customer Service ' +  + JSON.stringify(fac));

    return fac;
});

