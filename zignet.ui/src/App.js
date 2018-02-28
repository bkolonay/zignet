import React, { Component } from 'react';

import SuiteResults from './home/SuiteResults'
import ZigNetApi from './api/ZigNetApi'


class App extends Component {

  render() {
    const zigNetApi = new ZigNetApi();
    const latestSuiteResults = zigNetApi.getLatestSuiteResults();

    return (
      <div>
        <h1>ZigNet</h1>
        <SuiteResults suiteResults={latestSuiteResults} />
      </div>
    );
  }
}

export default App;
