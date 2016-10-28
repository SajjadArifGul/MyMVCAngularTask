function getallData() {
    //debugger;
    $http.get('../api/Customers/')
        .success(function (data, status, headers, config) {
            $scope.ListCustomer = data;
        })
        .error(function (data, status, headers, config) {
            $scope.message = 'Unexpected Error while loading data!!';
            $scope.result = "color-red";
            console.log($scope.message);
        });
};

$scope.getCustomer = function (custModel) {
    $http.get('/Home/GetbyID/' + custModel.Id)
    .success(function (data, status, headers, config) {
        //debugger;
        $scope.custModel = data;
        getallData();
        console.log(data);
    })
    .error(function (data, status, headers, config) {
        $scope.message = 'Unexpected Error while loading data!!';
        $scope.result = "color-red";
        console.log($scope.message);
    });
};
