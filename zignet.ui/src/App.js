import React, { Component } from 'react';
import { Route, Switch } from 'react-router-dom'
import LatestSuiteResults from './home/LatestSuiteResults'
import LatestSuiteResultsForSuite from './suite/LatestSuiteResultsForSuite'
import BadRoute from './common/BadRoute'

class App extends Component {

  render() {
    return (
      <div>
        <h1 className="text-center">ZigNet</h1>
        <Switch>
          <Route exact path="/" 
                 render={() => <LatestSuiteResults zigNetApi={this.props.zigNetApi} />} 
          />
          <Route path='/:suiteId' component={LatestSuiteResultsForSuite}/>
          <Route component={BadRoute} />
        </Switch>
      </div>
    );
  }
}

export default App;