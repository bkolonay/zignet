import React, { Component } from 'react';
import './App.css';

class App extends Component {

  getLatestSuiteResults() {
    return {
      totalPassedTests: 10,
      totalFailedTests: 25
    }
  }

  render() {
    return (
      <div className="App">
        <h1 className="App-title">ZigNet</h1>
      </div>
    );
  }
}

export default App;
