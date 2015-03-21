/// <reference path="Scripts/moment.min.js" />
/// <reference path="Scripts/angular.moment.min.js" />
/// <reference path="Scripts/angular.min.js" />

// Requirement 8
var statusApp = angular.module('statusApp', ['ngAnimate', 'angularMoment']);

statusApp.controller('statusCtrl', function ($scope, $timeout, statusService) {
    $scope.loaded = false;
    $scope.error = "";
    $scope.statuses = [];

    $timeout(function () {
        loadRemoteData();
    }, 2000);

    function applyRemoteData(newStatuses) {
        $scope.statuses = newStatuses;
        $scope.loaded = true;
    }

    function handleError(errorMessage) {
        $scope.error = errorMessage;
        $scope.loaded = true;
    }

    function loadRemoteData() {
        statusService.getStatuses()
            .catch(function(error){ handleError(error); })
            .then(function (newStatuses) { applyRemoteData(newStatuses); })
        ;
    }
});

statusApp.service(
    "statusService",
    function ($http, $q) {
        return ({
            getStatuses: getStatuses
        });
        function getStatuses() {
            var request = $http({
                method: "get",
                url: "https://api.myjson.com/bins/1cqjf",
                params: {
                    action: "get"
                }
            });
            return (request.then(handleSuccess, handleError));
        }

        function handleError(response) {
            if (!angular.isObject(response.data) || !response.data.message){
                return ($q.reject("An unknown error occurred."));
            }
            return ($q.reject(response.data.message));
        }

        function handleSuccess(response) {
            return (response.data);
        }
    }
);