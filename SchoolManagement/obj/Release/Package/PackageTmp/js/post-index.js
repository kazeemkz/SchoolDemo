

var module = angular.module("homeIndex", []);

module.config(function ($routeProvider) {

    $routeProvider.when("/", {
        controller: "postsController",
        templateUrl: "/templates/postsView.html"

    });

    $routeProvider.when("/newmessage", {
        controller: "newPostController",
        templateUrl: "/templates/newPostView.html"
    });

    $routeProvider.when("/message/:id", {
        controller: "singlePostController",
        templateUrl: "/templates/singlePostView.html"

    });

    $routeProvider.otherwise("/");

});

module.factory("dataService", function ($http, $q) {

    var _topics = [];
    var _Uniquetopic = [];
    var _isInt = false;
    var _isReady = function () {
        return _isInt;
    }

    //get unigue Topic
    var _getUniqueTopic = function (id) {

        var deferred = $q.defer();
       //  alert(id);
        $http.get("/api/posts?id=" + id)
    .then(function (result) {
        //success
        angular.copy(result.data, _Uniquetopic);
        //  _isInt = true;
        deferred.resolve();
        // $scope.isBusy = false;
    },

        function () {
            //error
            deferred.reject();

        });

        return deferred.promise;

    };


    var _getTopics = function () {

        var deferred = $q.defer();

        $http.get("/api/posts?includeReplies=true")
    .then(function (result) {
        //success
        angular.copy(result.data, _topics);
        _isInt = true;
        deferred.resolve();
        // $scope.isBusy = false;
    },

        function () {
            //error
            deferred.reject();

        });

        return deferred.promise;

    };

    var _addTopic = function (newTopic) {
        var deferred = $q.defer();


        $http.post("/api/posts", newTopic)
        .then(function (result) {
            //success
            var newlyCreatedTopic = result.data;
            //  $window.location = "#/";
            _topics.splice(0, 0, newlyCreatedTopic);
            deferred.resolve(newlyCreatedTopic);
        },

        function () {

            //failure
            deferred.reject();
        }

  )




        return deferred.promise;
    };


    function _findTopic(id) {

        var found = null;
        $.each(_topics, function (i, item) {

            if (item.id == id) {
                found = item;
                return false;
            }
        });



        return found;

    }
    var _getTopicById = function (id) {

        var deffered = $q.defer();

        if (_isReady()) {
            var topic = _findTopic(id);

            if (topic) {
                deffered.resolve(topic);
            } else {
                deffered.reject();
            }

        }

        else {
            _getTopics().then(

                function () {
                    var topic = _findTopic(id);

                    if (topic) {
                        deffered.resolve(topic);
                    } else {
                        deffered.reject();
                    }
                },
            function () {
                deffered.reject();
            });

        }





        return deferred.promise;
    };

    var _saveReply = function (Uniquetopic, newReply,id) {
               var deferred = $q.defer();
              // alert(id);
             //  $http.get("/api/posts?id=" + id)
               $http.post("api/posts?id=" +id, newReply).then(
        //$http.post("api/posts/" + topic.id + "/comments", newReply).then(

        function (result) {

           // if (Uniquetopic.Comments == null) Uniquetopic.Comments = [];
           //Uniquetopic.Comments.push(result.data);
            deferred.resolve(result.data);
        },
        function () {
            deferred.reject();
        });

        return deferred.promise;
    };



     var _getAdminStatus = function () {
               var deferred = $q.defer();
              // alert(id);
               $http.get("/api/posts?id=1&idi=2").then(
             //  $http.get("/Post/Gett").then(
        //$http.post("api/posts/" + topic.id + "/comments", newReply).then(

        function (result) {

           // if (Uniquetopic.Comments == null) Uniquetopic.Comments = [];
           //Uniquetopic.Comments.push(result.data);
            deferred.resolve(result.data);
        },
        function () {
            deferred.reject();
        });

        return deferred.promise;
    };














    return {
        topics: _topics,
        getTopics: _getTopics,
        addTopic: _addTopic,
        isReady: _isReady,
        getTopicById: _getTopicById,
        getUniqueTopic: _getUniqueTopic,
        Uniquetopic: _Uniquetopic,
        saveReply: _saveReply,
        getAdminStatus: _getAdminStatus
    };

});


//post-index

function postsController($scope, $http, dataService) {

    $scope.dataCount = 0;
    //$scope.data = [];
    $scope.data = dataService;
    $scope.isBusy = false;
    $scope.adminStatus = false;


    
    dataService.getAdminStatus()
    .then(

    function (result) {
              $scope.adminStatus = true;
        //success
        //  $window.location = "#/";
    },

    function () {
        //failure
        //alert("Could not save Topic!");
    });






    if (dataService.isReady() == false) {


        $scope.isBusy = true;
        dataService.getTopics()
            .then(function () {
                //success
                $scope.isBusy = false;
            },
            function () {
                //failure
                $scope.isBusy = false;
                alert("Could not load posts");
            })
        .then(function () {
            $scope.isBusy = false;
        });


    }
}

function newPostController($scope, $http, $window, dataService) {

    $scope.newPost = {};

    $scope.save = function () {

        dataService.addTopic($scope.newPost)
        .then(

        function () {
            //success
            $window.location = "#/";
        },

        function () {
            //failure
            alert("Could not save Topic!");
        })
    };
}


function singlePostController($scope, dataService, $window, $routeParams) {
   // alert("Hello");
    $scope.Uniquetopic = null;
    $scope.data = dataService;
    $scope.newReply = {};
    $scope.id = $routeParams.id;
    //alert($scope.id);
    dataService.getUniqueTopic($routeParams.id)
   // dataService.getTopicById($routeParams.id)
    .then(

    function (Uniquetopic) {
       // $scope.data = result;
        //successs
             // alert(Uniquetopic.PostID);
        $scope.Uniquetopic = Uniquetopic;
    },
    function () {
        //bad
        $window.location = "#/";
    });

    $scope.addReply = function () {

       
        dataService.saveReply($scope.Uniquetopic, $scope.newReply, $scope.id)
        .then(
        function () {
            //success
         
            $scope.newReply.Content = "";
            $window.location = "#/";
        },
        function () {
            alert("Cannot add reply");
        //errorsss
        }
        )
    };
}

