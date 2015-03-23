/// <reference path="Scripts/angular.min.js" />
/// <reference path="Scripts/moment.min.js" />
/// <reference path="Scripts/angular.moment.min.js" />

// Requirement 8
var statusApp = angular.module('statusApp', ['angularMoment']);

statusApp.controller('statusCtrl', function ($scope, $timeout, statusService) {
    $scope.loaded = false;
    $scope.error = "";
    $scope.statuses = [];

    $scope.noProblems = function () {
        return $scope.loaded && $scope.statuses.length == 0;
    }

    $scope.haveProblems = function () {
        return $scope.loaded && $scope.statuses.length > 0;
    }

    $scope.haveError = function () {
        return $scope.loaded && $scope.error;
    }

    function applyRemoteData(newStatuses) {
        $scope.loaded = true;
        $scope.statuses = newStatuses;
    }

    function handleError(errorMessage) {
        $scope.loaded = true;
        $scope.error = errorMessage;
    }

    function loadRemoteData() {
        statusService.getStatuses()
            .catch(function(error){ handleError(error); })
            .then(function (newStatuses) { applyRemoteData(newStatuses); })
        ;
    }

    loadRemoteData();
});

statusApp.service(
    "statusService",
    function ($http, $q) {
        return ({
            getStatuses: getStatuses
        });
        function getStatuses() {
            var request = $http({
                method: "jsonp",
                url: "https://status.campaignmonitor.com/api/issues/current?callback=JSON_CALLBACK"
            });
            return (request.then(handleSuccess, handleError));
        }

        function handleError(response) {
            if (!angular.isObject(response.data) || !response.data.message) {
                return ($q.reject("An unknown error occurred."));
            }
            return ($q.reject(response.data.message));
        }

        function handleSuccess(response) {
            return (response.data);
        }
    }
);

// Uncomment this and comment the service above to test.
//statusApp.service(
//    "statusService",
//    function ($http, $q) {
//        return ({
//            getStatuses: getStatuses
//        });
//        function getStatuses() {
//            var request = $http({
//                method: "get",
//                url: "https://api.myjson.com/bins/1cqjf",
//                params: {
//                    action: "get"
//                }
//            });
//            return (request.then(handleSuccess, handleError));
//        }

//        function handleError(response) {
//            if (!angular.isObject(response.data) || !response.data.message){
//                return ($q.reject("An unknown error occurred."));
//            }
//            return ($q.reject(response.data.message));
//        }

//        function handleSuccess(response) {
//            return (response.data);
//        }
//    }
//);