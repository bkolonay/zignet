import React, { Component } from 'react';
import LatestTestResultsList from './LatestTestResultsList'

class LatestTestResults extends Component {
  constructor(props) {
    super(props);

    this.zigNetApi = this.props.zigNetApi;
    this.state = { latestTestResults: [] }
    this.suiteId = this.props.match.params.suiteId;
  }

  componentDidMount() {
    this._getLatestTestResultsForSuite();
    this.intervalId = setInterval(() => this._getLatestTestResultsForSuite(), 5000);
  }

  componentWillUnmount() {
    clearInterval(this.intervalId);
  }

  _getLatestTestResultsForSuite() {
    this.zigNetApi.getLatestTestResultsForSuite(this.suiteId)
      .then(response => {
        this.setState({
          latestTestResults: response
        })
      })
      .catch(error => alert(error));
  }

  render() {
    return (
      <div>
        <LatestTestResultsList testResults={this.state.latestTestResults} />
      </div>
    );
  }
}

export default LatestTestResults;
