import React, { Component } from 'react';
import LatestTestResultsList from './LatestTestResultsList'

class TestList extends Component {
  constructor() {
    super();

    this.state = { 
      suiteName: '',
      latestTestResults: []
    }
  }

  componentDidMount() {
    var getData = () => {
      this.props.getTests()
        .then(response => {
          this.setState({
            suiteName: response.SuiteName,
            latestTestResults: response.LatestTestResults
          })
        })
        .catch(error => console.log('Failed to fetch latest test results: ' + error))
    }

    getData();
    this.intervalId = setInterval(() => getData(), 20000);
  }

  componentWillUnmount() {
    clearInterval(this.intervalId);
  }

  render() {
    return (
      <div>
        <LatestTestResultsList 
          suiteName={this.state.suiteName}
          testResults={this.state.latestTestResults} />
      </div>
    );
  }
}

export default TestList;
