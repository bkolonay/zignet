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
          <Route exact path="/" 
                 render={() => <LatestSuiteResults zigNetApi={this.props.zigNetApi} />} 
          />
          <Route path='/:suiteId'
                 render={(props) => <LatestTestResults {...props} zigNetApi={this.props.zigNetApi} />}
          />
          <Route component={BadRoute} />
        </Switch>
      </div>
    );
  }
}

export default App;