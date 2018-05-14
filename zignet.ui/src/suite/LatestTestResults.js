import React, { Component } from 'react';
import LatestTestResultsList from './LatestTestResultsList'

class LatestTestResults extends Component {
  constructor(props) {
    super(props);

    this.zigNetApi = this.props.zigNetApi;
    this.state = { 
      suiteName: '',
      latestTestResults: []
    }
    this.suiteId = this.props.match.params.suiteId;
  }

  componentDidMount() {
    this._getLatestTestResultsForSuite();
    this.intervalId = setInterval(() => this._getLatestTestResultsForSuite(), 10000);
  }

  componentWillUnmount() {
    clearInterval(this.intervalId);
  }

  _getLatestTestResultsForSuite() {
    this.zigNetApi.getLatestTestResultsForSuite(this.suiteId)
      .then(response => {
        this.setState({
          suiteName: response.SuiteName,
          latestTestResults: response.LatestTestResults
        })
      })
      .catch(error => alert(error));
  }

  render() {
    return (
      <div>
        <LatestTestResultsList suiteName={this.state.suiteName} testResults={this.state.latestTestResults} />
      </div>
    );
  }
}

export default LatestTestResults;
