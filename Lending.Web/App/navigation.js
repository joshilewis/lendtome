
lendtomeControllers.controller('navController',
    ['$scope', '$auth', 'SatellizerStorage', '$location', 
  function ($scope, $auth, storage, $location) {
      $scope.isAuthenticated = $auth.isAuthenticated();
      $scope.userName = storage.get('userName');

      $scope.signin = function (provider) {
          $location.path('/authenticating');
          $auth.authenticate(provider).then(function () {
              var payload = $auth.getPayload();
              var userName = payload.Claims[0].Value;
              storage.set('userName', userName);
              storage.set('userId', payload.Claims[1].Value);
              $scope.isAuthenticated = true;
              $scope.userName = storage.get('userName');
              $location.path('/books');
          },
          function() {
              $location.path('/autherror');
          });
      };

      $scope.signout = function () {
          $auth.logout();
          $scope.isAuthenticated = false;
          $scope.userName = '';
          $location.path('/');
      }

  }]);
