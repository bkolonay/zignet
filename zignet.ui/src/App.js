import React, { Component } from 'react';
import { Route, Switch } from 'react-router-dom'
import LatestSuiteResults from './home/LatestSuiteResults'
import LatestTestResults from './suite/LatestTestResults'
import BadRoute from './common/BadRoute'

class App extends Component {

  render() {
    return (
      <div>
        <Switch>
          <Route exact path="/" render={(props) => <LatestSuiteResults {...props} />} />
          <Route path='/:suiteId' render={(props) => <LatestTestResults {...props} />} />
          <Route component={BadRoute} />
        </Switch>
      </div>
    );
  }
}

export default App;