import React, { Component } from 'react';
import ZigNetApi from '../api/ZigNetApi'
import LatestTestResultsList from './LatestTestResultsList'

class LatestTestResults extends Component {
  constructor(props) {
    super(props);

    if (this.props.location.search.indexOf('group=true') !== -1)
      this.testResultsGrouped = true;
    else
      this.testResultsGrouped = false;

    this.zigNetApi = new ZigNetApi(process.env.REACT_APP_API_BASE_URL + 'api/');
    this.state = { 
      suiteName: '',
      latestTestResults: []
    }
    this.suiteId = this.props.match.params.suiteId;
  }

  componentDidMount() {
    this._getLatestTestResultsForSuite(this.suiteId, this.testResultsGrouped);
    this.intervalId = setInterval(() => this._getLatestTestResultsForSuite(this.suiteId, this.testResultsGrouped), 20000);
  }

  componentWillUnmount() {
    clearInterval(this.intervalId);
  }

  _getLatestTestResultsForSuite(suiteId, testResultsGrouped) {
    this.zigNetApi.getLatestTestResultsForSuite(suiteId, testResultsGrouped)
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
        <LatestTestResultsList 
          suiteName={this.state.suiteName}
          suiteId={this.suiteId}
          testResults={this.state.latestTestResults}
          testResultsGrouped={this.testResultsGrouped} />
      </div>
    );
  }
}

export default LatestTestResults;
