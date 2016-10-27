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
        ModifiedOn:''
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
        $scope.Customer.ModifiedOn = ''
    };

    //Add New Item
    $scope.save = function () {
        if ($scope.Customer.Name != "" &&
            $scope.Customer.Details != "" &&
            $scope.Customer.Address != "" &&
            $scope.Customer.Contact != "" &&
            $scope.Customer.IsActive != "") {

            // or you can call Http request using $http
            $http({
                method: 'POST',
                url: '../api/Customers/',
                data: $scope.Customer
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
        $scope.Customer = { ID: data.ID, Name: data.Name, Details: data.Details, Address: data.Address, Contact: data.Contact };
    };

    // Cancel product details
    $scope.cancel = function () {
        $scope.clear();
    };

    // Update product details
    $scope.update = function () {
        if ($scope.Customer.ID != "" &&
            $scope.Customer.Name != "" &&
            $scope.Customer.Details != "" &&
            $scope.Customer.Address != "" &&
            $scope.Customer.Contact != "" &&
            $scope.Customer.IsActive != "") {
            $http({
                method: 'PUT',
                url: '../api/Customers/' + $scope.Customer.ID,
                data: $scope.Customer
            }).then(function successCallback(response) {
                $scope.customersData = response.data;
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
    return fac;
});