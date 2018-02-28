import React, { Component } from 'react';
import './App.css';

import SuiteResults from './home/SuiteResults'
import ZigNetApi from './api/ZigNetApi'


class App extends Component {

  render() {
    const zignetApi = new ZigNetApi();
    const latestSuiteResults = zignetApi.getLatestSuiteResults();

    return (
      <div className="App">
        <h1 className="App-title">ZigNet</h1>
        <SuiteResults suiteResults={latestSuiteResults} />
      </div>
    );
  }
}

export default App;
